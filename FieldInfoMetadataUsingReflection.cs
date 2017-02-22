static class PivotFieldInfoHelper
{
    // This is current code. This method extracts metadata of source property, using reflection and private fields.
    private static TAtt GetAttributeUsingReflection<TAtt>(this IPivotFieldInfo fieldInfo) where TAtt : Attribute
    {
        // This is for PropertyInfoFieldInfo
        if (fieldInfo is PropertyInfoFieldInfo)
        {
            PropertyInfo propertyInfo = (fieldInfo as PropertyInfoFieldInfo).PropertyInfo;
            return propertyInfo.GetCustomAttribute<TAtt>();
        }

        object propertyInfoField = ReflectionHelper.GetPrivateFieldValue(fieldInfo, "propertyInfo");

        // This is for QueryableFieldDescription
        if (propertyInfoField is PropertyInfo)
        {
            return (propertyInfoField as PropertyInfo).GetCustomAttribute<TAtt>();
        }

        // this is for DateTimePropertyFieldInfo
        if (propertyInfoField is IPivotFieldInfo)
        {
            // Recursive call
            return (propertyInfoField as IPivotFieldInfo).GetAttributeUsingReflection<TAtt>();
        }

        // This is for PropertyDescriptorFieldInfo
        object propertyDescriptorField = ReflectionHelper.GetPrivateFieldValue(fieldInfo, "propertyDescriptor");
        if (propertyDescriptorField is PropertyDescriptor)
        {
            return (propertyDescriptorField as PropertyDescriptor).Attributes
                .OfType<TAtt>()
                .FirstOrDefault();
        }

        return null;
    }
}