using Newtonsoft.Json;

namespace ManageableHangfire.Core
{
    public static class Extensions
    {
        public static string ToJson(this object pObject)
        {
            string strObj = JsonConvert.SerializeObject(pObject);
            return strObj;
        }
    }
}
