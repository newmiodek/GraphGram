namespace GraphGram;
public class ExponentNotationNumber {
    /* The intention behind having these values be ints is that this will
     * fix the issue of inexact decimal point representation by floats,
     * which has caused problems with too many decimal digits in the axis marks.
     */
    private int significand;
    private int exponent;

    public ExponentNotationNumber(int significand, int exponent) {
        this.significand = significand;
        this.exponent = exponent;
    }

    public int GetSignificand() {
        return significand;
    }
    public int GetExponent() {
        return exponent;
    }
    public void SetSignificand(int significand) {
        this.significand = significand;
    }
    public void SetExponent(int exponent) {
        this.exponent = exponent;
    }

    public float GetFloatValue() {
        return significand * MathF.Pow(10, exponent);
    }

    public double GetDoubleValue() {
        return significand * Math.Pow(10, exponent);
    }

    public void Add(ExponentNotationNumber aNumber) {
        ExponentNotationNumber number = aNumber.DeepCopy();
        while(number.GetExponent() > exponent) {
            number.SetExponent(number.GetExponent() - 1);
            number.SetSignificand(number.GetSignificand() * 10);
        }
        while(number.GetExponent() < exponent) {
            exponent--;
            significand *= 10;
        }
        significand += number.GetSignificand();
    }

    public void Subtract(ExponentNotationNumber aNumber) {
        ExponentNotationNumber number = aNumber.DeepCopy();
        while(number.GetExponent() > exponent) {
            number.SetExponent(number.GetExponent() - 1);
            number.SetSignificand(number.GetSignificand() * 10);
        }
        while(number.GetExponent() < exponent) {
            exponent--;
            significand *= 10;
        }
        significand -= number.GetSignificand();
    }

    public ExponentNotationNumber DeepCopy() {
        return new ExponentNotationNumber(significand, exponent);
    }

    public override string ToString() {
        string toStringed;
        if(exponent >= 0) {
            toStringed = significand.ToString();
            for(int i = 0; i < exponent; i++) {
                toStringed += "0";
            }
        }
        else {
            bool isNegative = significand < 0;
            int minuslessSignificand = significand * (isNegative ? -1 : 1);
            int decimalDigits = -exponent;

            if(decimalDigits >= minuslessSignificand.ToString().Length) {
                toStringed = (isNegative ? "-" : "") + "0.";
                for(int i = 0; i < decimalDigits - minuslessSignificand.ToString().Length; i++) {
                    toStringed += "0";
                }
                toStringed += minuslessSignificand.ToString();
            }
            else {
                toStringed = significand.ToString();
                toStringed = toStringed.Insert(toStringed.Length - decimalDigits, ".");
            }
        }
        return toStringed;
    }
}
