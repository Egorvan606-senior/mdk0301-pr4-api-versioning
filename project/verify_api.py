"""Start the local API and verify v1/v2 compatibility with real HTTP requests."""
import json
import os
import socket
import subprocess
import time
import urllib.error
import urllib.request
from pathlib import Path

ROOT = Path(__file__).resolve().parent
with socket.socket() as sock:
    sock.bind(("127.0.0.1", 0))
    PORT = sock.getsockname()[1]
BASE = f"http://127.0.0.1:{PORT}"
creationflags = getattr(subprocess, "CREATE_NO_WINDOW", 0)
proc = subprocess.Popen(
    ["dotnet", "run", "--no-build", "--project", str(ROOT / "ApiVersioningLab.Ivanov.csproj"), "--urls", BASE],
    cwd=ROOT,
    env={**os.environ, "ASPNETCORE_ENVIRONMENT": "Development"},
    stdout=subprocess.DEVNULL,
    stderr=subprocess.DEVNULL,
    creationflags=creationflags,
)


def request(path):
    try:
        with urllib.request.urlopen(BASE + path, timeout=2) as response:
            return response.status, json.load(response), dict(response.headers)
    except urllib.error.HTTPError as error:
        return error.code, None, dict(error.headers)


try:
    for _ in range(60):
        try:
            request("/api/v1/products")
            break
        except Exception:
            if proc.poll() is not None:
                raise RuntimeError("API process exited before serving requests")
            time.sleep(0.25)
    else:
        raise RuntimeError("API did not start in time")

    status1, v1, headers1 = request("/api/v1/products")
    status2, v2, headers2 = request("/api/v2/products")
    assert status1 == status2 == 200
    assert len(v1) == len(v2) == 3
    base_fields = {"id", "title", "price"}
    added_fields = {"currency", "inStock", "category", "apiVersionInfo"}
    assert all(set(p) == base_fields for p in v1)
    assert all(set(p) == base_fields | added_fields for p in v2)
    assert all({k: p[k] for k in base_fields} == v1[i] for i, p in enumerate(v2))
    assert v1 == [
        {"id": 1, "title": "Ноутбук Lenovo IdeaPad 5", "price": 64990},
        {"id": 2, "title": "Смартфон Samsung Galaxy A55", "price": 32490},
        {"id": 3, "title": "Беспроводные наушники Sony WH-1000XM5", "price": 29990},
    ]
    assert [(p["currency"], p["inStock"], p["category"]) for p in v2] == [
        ("RUB", True, "Ноутбуки"),
        ("RUB", False, "Смартфоны"),
        ("RUB", True, "Аудио"),
    ]
    assert "1.0" in headers1.get("api-supported-versions", "")
    assert "2.0" in headers2.get("api-supported-versions", "")
    print("PASS: v1 and v2 return HTTP 200; all 3 products match the original Swagger screenshots")
    print("PASS: v1 contract has 3 fields, v2 adds 4 fields")
    print("PASS: v1 fields and values are unchanged in v2")
    print("PASS: supported API versions are reported")

    for version in ("v1", "v2"):
        status, product, _ = request(f"/api/{version}/products/2")
        assert status == 200 and product["id"] == 2 and product["title"] == "Смартфон Samsung Galaxy A55"
    print("PASS: GET by id works in both versions")

    for version in ("v1", "v2"):
        status, _, _ = request(f"/api/{version}/products/999999")
        assert status == 404
    print("PASS: unknown product returns HTTP 404")

    for version in ("v1", "v2"):
        status, schema, _ = request(f"/swagger/{version}/swagger.json")
        assert status == 200 and schema["openapi"]
    print("PASS: both Swagger documents are available")
    print("RESULT: 7/7 checks passed")
finally:
    proc.terminate()
    try:
        proc.wait(timeout=5)
    except subprocess.TimeoutExpired:
        proc.kill()
