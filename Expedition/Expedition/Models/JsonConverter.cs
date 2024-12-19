using Expedition.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class InventoryItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        
        return typeof(IInventoryItem).IsAssignableFrom(objectType);
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var item = (IInventoryItem)value;
        JObject jo = new JObject
        {
            { "Type", item.GetType().Name }, 
            { "Name", item.Name },
            { "Description", item.Description },
            { "MaximumCount", item.MaximumCount },
            { "IsVisible", item.IsVisible },
            { "SecondName", item.SecondName }
        };

        if (item is Food food)
        {
            jo.Add("Saturation", food.Saturation);
        }
        else if (item is HealthKit healthKit)
        {
            jo.Add("RecoverableHealth", healthKit.RecoverableHealth);
        }
        else if (item is QuestItem questItem)
        {
            jo.Add("Action", questItem.Action);
        }

        jo.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string type = jo["Type"].ToString();

        IInventoryItem item = type switch
        {
            nameof(Food) => new Food(),
            nameof(HealthKit) => new HealthKit(),
            nameof(QuestItem) => new QuestItem(),
            _ => throw new NotSupportedException($"Тип '{type}' не поддерживается.")
        };

        serializer.Populate(jo.CreateReader(), item);
        return item;
    }
}
