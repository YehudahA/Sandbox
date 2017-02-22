static class PivotFieldInfoHelper
{
    // This code is better.
    private static TAtt GetAttributeUsingInterface<TAtt>(this IPivotFieldInfo fieldInfo) where TAtt : Attribute
    {
        // This is for PropertyInfoFieldInfo and QueryableFieldDescription
        if (fieldInfo is IPropertyInfoSourceFieldInfo)
        {
            PropertyInfo propertyInfo = (fieldInfo as PropertyInfoFieldInfo).PropertyInfo;
            return propertyInfo.GetCustomAttribute<TAtt>();
        }

        // this is for DateTimePropertyFieldInfo, which is wrapper for PivotFieldInfo
        if (fieldInfo is IPivotFieldSourceFieldInfo)
        {
            IPivotFieldInfo innerFieldInfo = (fieldInfo as IPivotFieldSourceFieldInfo).FieldInfo;
			
			// Recursive call
            return innerFieldInfo.GetAttributeUsingInterface<TAtt>();
        }

        // This is for PropertyDescriptorFieldInfo
        if (propertyDescriptorField is IPropertyDescriptorFieldInfo)
        {
            return (propertyDescriptorField as IPropertyDescriptorFieldInfo).PropertyDescriptor
                .Attributes
                .OfType<TAtt>()
                .FirstOrDefault();
        }

        return null;
    }
}