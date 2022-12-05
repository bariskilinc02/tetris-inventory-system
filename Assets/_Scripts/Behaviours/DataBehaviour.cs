using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

  public static class DataBehaviour
  { 
    public static string Serialize<T>(T toSerialize)
    {
      return JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
    }

    public static T DeSerialize<T>(string toDeSerialize)
    {
      return JsonConvert.DeserializeObject<T>(toDeSerialize);
    }

}
