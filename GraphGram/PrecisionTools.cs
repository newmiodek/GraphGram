namespace GraphGram;
public static class PrecisionTools {
    public static string DPToString(double val, int dp) {
        string processedBody;
        string body = val.ToString(Constants.NO_SCI_FORMAT);
        int dotIndex = body.Length;
        for(int i = 0; i < body.Length; i++) {
            if(body[i] == '.') {
                dotIndex = i;
                break;
            }
        }
        if(dp > 0) {
            int originalDP = Math.Max(body.Length - dotIndex - 1, 0);

            if(originalDP < dp) {
                processedBody = body + (dotIndex == body.Length ? "." : "");
                for(int i = dp - originalDP; i > 0; i--) {
                    processedBody += "0";
                }
            }
            else if(originalDP == dp) {
                processedBody = body;
            }
            else {  // originalDP > dp
                processedBody = val.ToString("0." + new string('#', dp));
                bool hasDot = false;
                for(int i = 0; i < processedBody.Length; i++) {
                    if(processedBody[i] == '.') {
                        hasDot = true;
                        break;
                    }
                }
                if(!hasDot) processedBody += ".";
                while(processedBody.Length < dotIndex + 1 + dp) processedBody += "0";
            }
        }
        else if(dp == 0) {
            processedBody = val.ToString("#");
        }
        else {  // dp < 0
            double shifter = 1.0;
            for(int i = 0; i < -dp; i++) shifter *= 10.0;
            string shiftedVal = (val / shifter).ToString("#");
            if(shiftedVal.Length == 0) shiftedVal = "0";
            processedBody = (int.Parse(shiftedVal) * ((int)shifter)).ToString();
        }
        return processedBody;
    }
}
