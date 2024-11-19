import requests

# Base URL for the service
URL = "http://localhost:5278/api/v1/units"

# 1. Single conversion (0°F to °C)
print("\n1. Single Temperature Conversion (0°F to °C):")
params = {'value': 0, 'fromUnit': 'f', 'toUnit': 'c'}
response = requests.get(f"{URL}/convert", params=params)
print(response.json())

# 2. Bulk conversion
print("\n2. Bulk Temperature Conversion:")
data = {
    "conversions": [
        {"value": 32, "fromUnit": "f", "toUnit": "c"},
        {"value": 100, "fromUnit": "c", "toUnit": "f"}
    ]
}
response = requests.post(f"{URL}/convert/bulk", json=data)
print(response.json())

# 3. Get unit information
print("\n3. Supported Units Information:")
response = requests.get(f"{URL}/info")
print(response.json())