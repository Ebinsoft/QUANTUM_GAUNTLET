using System;

[Serializable]
public class SerializedClass
{
    public string className;
    public string qualifiedName;

    public SerializedClass(Type classType)
    {
        this.className = classType.ToString();
        this.qualifiedName = classType.AssemblyQualifiedName;
    }

    public System.Object CreateInstance()
    {
        return Activator.CreateInstance(Type.GetType(qualifiedName));
    }
}
