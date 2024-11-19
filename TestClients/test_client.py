import requests
import json

BASE_URL = "http://localhost:5278/api/v1/units"

def test_single_conversion():
    """Test single temperature conversion"""
    print("\nTesting single conversion...")
    
    # Parameters for the conversion
    params = {
        'value': 32,
        'fromUnit': 'c',
        'toUnit': 'f'
    }
    
    try:
        response = requests.get(f"{BASE_URL}/convert", params=params)
        if response.status_code == 200:
            result = response.json()
            print(f"Converted {result['originalValue']} {result['originalUnit']} "
                  f"to {result['convertedValue']} {result['targetUnit']}")
        else:
            print(f"Error: {response.status_code}")
            print(response.text)
    except Exception as e:
        print(f"Exception occurred: {str(e)}")

def test_bulk_conversion():
    """Test bulk temperature conversion"""
    print("\nTesting bulk conversion...")
    
    # Data for bulk conversion
    data = {
        "conversions": [
            {"value": 0, "fromUnit": "C", "toUnit": "F"},
            {"value": 100, "fromUnit": "C", "toUnit": "F"}
        ]
    }
    
    try:
        response = requests.post(
            f"{BASE_URL}/convert/bulk",
            json=data,
            headers={'Content-Type': 'application/json'}
        )
        
        if response.status_code == 200:
            results = response.json()
            for result in results:
                print(f"Converted {result['originalValue']} {result['originalUnit']} "
                      f"to {result['convertedValue']} {result['targetUnit']}")
        else:
            print(f"Error: {response.status_code}")
            print(response.text)
    except Exception as e:
        print(f"Exception occurred: {str(e)}")

def test_unit_info():
    """Test getting unit information"""
    print("\nTesting unit information...")
    
    try:
        response = requests.get(f"{BASE_URL}/info")
        if response.status_code == 200:
            units = response.json()
            print("Supported units and their ranges:")
            for unit in units:
                print(f"{unit['unit']}: {unit['minimumValue']} to {unit['maximumValue']}")
        else:
            print(f"Error: {response.status_code}")
            print(response.text)
    except Exception as e:
        print(f"Exception occurred: {str(e)}")

if __name__ == "__main__":
    print("Temperature Conversion Service Test Client (Python)")
    test_single_conversion()
    test_bulk_conversion()
    test_unit_info()