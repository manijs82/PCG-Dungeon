using System.Collections.Generic;

public static class ServiceLocator
{
    private static List<Service> Services = new();

    public static TileSetProvider TileSetProvider;
    public static PerlinNoiseProvider PerlinNoiseProvider;

    public static T FindService<T>() where T : Service
    {
        foreach (var service in Services)
        {
            if (service is T s)
                return s;
        }

        return null;
    }
    
    public static void AddService(Service service)
    {
        Services.Add(service);
    }
    
    public static void RemoveService(Service service)
    {
        Services.Remove(service);
    }
}