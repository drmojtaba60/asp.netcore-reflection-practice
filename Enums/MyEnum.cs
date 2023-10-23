using Microsoft.OpenApi.Attributes;

namespace asp.netcore_reflection_practice.Enums
{
    public enum MyEnum01
    {
        Value1,
        Value2,
        Value3,
    }
    public enum MyEnum02
    {
        [Display("مقدار 1")]
        Value1,
        [Display("مقدار 2")]
        Value2,
        [Display("مقدار 3")]
        Value3,
    }
}
