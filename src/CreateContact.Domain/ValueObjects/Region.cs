using CreateContact.Domain.Exceptions;

namespace CreateContact.Domain.ValueObjects;

public class Region
{
    public int DddCode { get; }
    public string Name { get; }

    private static readonly Dictionary<string, List<int>> ValidRegions = new()
        {
            { "Centro-Oeste", new List<int> { 61, 62, 64, 65, 66, 67 } },
            { "Nordeste", new List<int> { 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 98, 99 } },
            { "Norte", new List<int> { 63, 68, 69, 91, 92, 93, 94, 95, 96, 97 } },
            { "Sudeste", new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35, 37, 38 } },
            { "Sul", new List<int> { 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55 } }
        };

    private Region(int dddCode, string name)
    {
        DddCode = dddCode;
        Name = name;
    }

    public static Region Create(int dddCode)
    {
        if (!IsValidDddCode(dddCode, out var regionName))
            throw new InvalidDDDException(dddCode);

        return new Region(dddCode, regionName);
    }

    private static bool IsValidDddCode(int dddCode, out string regionName)
    {
        regionName = ValidRegions.FirstOrDefault(region => region.Value.Contains(dddCode)).Key;
        return regionName != null;
    }
}
