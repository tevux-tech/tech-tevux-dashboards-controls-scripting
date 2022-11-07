namespace Tech.Tevux.Dashboards.Controls;

public class AutoConverter {
    public static bool TryGetAsText(object input, out string inputAsText) {
        var success = false;

        if (input is string inputString) {
            inputAsText = inputString;
            success = true;
        } else {
            inputAsText = "";
        }

        return success;
    }

    public static bool TryGetAsNumber(object input, out decimal inputAsNumber) {
        var success = false;

        if (input is float floatValue) {
            inputAsNumber = (decimal)floatValue;
            success = true;
        } else if (input is double doubleValue) {
            inputAsNumber = (decimal)doubleValue;
            success = true;
        } else if (input is byte byteValue) {
            inputAsNumber = byteValue;
            success = true;
        } else if (input is int intValue) {
            inputAsNumber = intValue;
            success = true;
        } else if (input is long longValue) {
            inputAsNumber = longValue;
            success = true;
        } else {
            inputAsNumber = 0;
        }

        return success;
    }
}
