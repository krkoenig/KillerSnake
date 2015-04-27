using System;
using System.Collections.Generic;

namespace scMessage
{
    [Serializable]
    public class message
    {
        public string messageText;
        private List<scObject> scObjects = new List<scObject>();
        public int messageID;

        public message(string x)
        {
            messageText = x;
        }

        public void addSCObject(scObject x)
        {
            scObjects.Add(x);
        }

        public scObject getSCObject(string x)
        {
            for (int i = 0; i < scObjects.Count; i++)
            {
                if (scObjects[i].name == x)
                    return scObjects[i];
            }
            return null;
        }

        public scObject getSCObject(int x)
        {
            return scObjects[x];
        }

        public int getSCObjectCount()
        {
            return scObjects.Count;
        }
    }

    [Serializable]
    public class scObject
    {
        public string name;

        private List<scString> stringL = new List<scString>();
        private List<scBool> boolL = new List<scBool>();
        private List<scInt> intL = new List<scInt>();
        private List<scLong> longL = new List<scLong>();
        private List<scFloat> floatL = new List<scFloat>();
        private List<scDouble> doubleL = new List<scDouble>();
        private List<scObject> objectL = new List<scObject>();

        public void addSCObject(scObject x)
        {
            objectL.Add(x);
        }

        public scObject getSCObject(string x)
        {
            for (int i = 0; i < objectL.Count; i++)
            {
                if (objectL[i].name == x)
                    return objectL[i];
            }
            return null;
        }

        public scObject getSCObject(int x)
        {
            return objectL[x];
        }

        public int getSCObjectCount()
        {
            return objectL.Count;
        }

        public scObject(string x)
        {
            name = x;
        }

        public void addString(string x, string y)
        {
            stringL.Add(new scString(x, y));
        }

        public string getString(string x)
        {
            for (int i = 0; i < stringL.Count; i++)
            {
                if (stringL[i].name == x)
                    return stringL[i].value;
            }
            return null;
        }

        public void addBool(string x, bool y)
        {
            boolL.Add(new scBool(x, y));
        }

        public bool getBool(string x)
        {
            for (int i = 0; i < boolL.Count; i++)
            {
                if (boolL[i].name == x)
                    return boolL[i].value;
            }
            return false;
        }

        public void addInt(string x, int y)
        {
            intL.Add(new scInt(x, y));
        }

        public int getInt(string x)
        {
            for (int i = 0; i < intL.Count; i++)
            {
                if (intL[i].name == x)
                    return intL[i].value;
            }
            return 0;
        }

        public void addLong(string x, long y)
        {
            longL.Add(new scLong(x, y));
        }

        public long getLong(string x)
        {
            for (int i = 0; i < longL.Count; i++)
            {
                if (longL[i].name == x)
                    return longL[i].value;
            }
            return 0;
        }

        public void addFloat(string x, float y)
        {
            floatL.Add(new scFloat(x, y));
        }

        public float getFloat(string x)
        {
            for (int i = 0; i < floatL.Count; i++)
            {
                if (floatL[i].name == x)
                    return floatL[i].value;
            }
            return 0F;
        }

        public void addDouble(string x, double y)
        {
            doubleL.Add(new scDouble(x, y));
        }

        public double getDouble(string x)
        {
            for (int i = 0; i < doubleL.Count; i++)
            {
                if (doubleL[i].name == x)
                    return doubleL[i].value;
            }
            return 0.0;
        }
    }

    [Serializable]
    public class scBool
    {
        public string name;
        public bool value;

        public scBool(string x, bool y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scDouble
    {
        public string name;
        public double value;

        public scDouble(string x, double y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scFloat
    {
        public string name;
        public float value;

        public scFloat(string x, float y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scInt
    {
        public string name;
        public int value;

        public scInt(string x, int y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scLong
    {
        public string name;
        public long value;

        public scLong(string x, long y)
        {
            name = x;
            value = y;
        }
    }

    [Serializable]
    public class scString
    {
        public string name;
        public string value;

        public scString(string x, string y)
        {
            name = x;
            value = y;
        }
    }
}
