
using System.Globalization;

namespace Taxi;

class Program
{
    static List<Taxi> taxis = new List<Taxi>();
    
    static void Main(string[] args)
    {
        Console.WriteLine("adads");
        ReadAndParseFile();
        HarmadikFeladat(); 
        NegyedikFeladat();
        OtodikFeladat();
        HatodikFeladat();
        HetedikFeladat();
        NyolcadikFeladat();
    }
    
    static void ReadAndParseFile()
    {
        string[] lines = File.ReadAllLines("../../../fuvar.csv");
        lines = lines.Skip(1).ToArray();
        foreach (string line in lines)
        {
            string[] values = line.Split(';');
            Taxi taxi = new Taxi(
                values[0], 
                DateTime.Parse(values[1]), 
                int.Parse(values[2]), 
                // Smth dumb with the comma and dot, needs to be replaced in order to parse it into double
                double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.6, 
                double.Parse(values[4].Replace(',', '.'), CultureInfo.InvariantCulture), 
                double.Parse(values[5].Replace(',', '.'), CultureInfo.InvariantCulture), 
                values[6]
                );
            taxis.Add(taxi);
        }
    }

    static void HarmadikFeladat()
    {
        int fuvarokSzama = taxis.Count;
        Console.WriteLine($"3. feladat: {fuvarokSzama} fuvar");
    }

    static void NegyedikFeladat()
    {
        int fuvarokSzama = 0;
        double bevetel = 0;
        foreach (Taxi taxi in taxis)
        {
            if (taxi.taxiId == "6185")
            {
                fuvarokSzama++;
                bevetel += taxi.taxiViteldij;
            }
        }
        Console.WriteLine($"4. feladat: {fuvarokSzama} fuvar alatt: {bevetel}$");
    
    }

    static void OtodikFeladat()
    {
        Console.WriteLine("5. feladat: ");
        Dictionary<string, int> fizetesModok = new Dictionary<string, int>();
        foreach (Taxi taxi in taxis)
        {
            if (!fizetesModok.ContainsKey(taxi.taxiFizetesModja))
            {
                fizetesModok.Add(taxi.taxiFizetesModja, 1);
            }
            else
            {
                fizetesModok[taxi.taxiFizetesModja]++;
            }
        }
        foreach (KeyValuePair<string, int> fizetesMod in fizetesModok)
        {
            Console.WriteLine($"\t{fizetesMod.Key}: {fizetesMod.Value} fuvar");
        }
    }
    
    static void HatodikFeladat()
    {
        Console.WriteLine("6. feladat: ");
        double osszTavolsag = 0;
        foreach (Taxi taxi in taxis)
        {
            osszTavolsag += taxi.taxiTavolsag;
        }
        Console.WriteLine($"\t{Math.Round(osszTavolsag, 2)} km");
    }

    static void HetedikFeladat()
    {
       Console.WriteLine("7. feladat: Leghosszabb fuvar:");

       Taxi leghosszabbFuvar = taxis.OrderByDescending(t=>t.taxiIdotartam).First();
       
       Console.WriteLine($"\tFuvar hossza: {leghosszabbFuvar.taxiIdotartam} másodperc");
       Console.WriteLine($"\tTaxi azonosító: {leghosszabbFuvar.taxiId}");
       Console.WriteLine($"\tMegtett távolság: {Math.Round(leghosszabbFuvar.taxiTavolsag, 2)} km");
       Console.WriteLine($"\tViteldíj: {leghosszabbFuvar.taxiViteldij}$");

    }
    
    static void NyolcadikFeladat()
    {
        Console.WriteLine("8. feladat: hibak.txt");
        StreamWriter sw = new StreamWriter("../../../hibak.txt");
        List<Taxi> hibasTaxis = new List<Taxi>();
        
        foreach (Taxi taxi in taxis)
        {
            if (taxi is { taxiIdotartam: > 0, taxiViteldij: > 0.0, taxiTavolsag: 0.0 } )
            {
                hibasTaxis.Add(taxi);
            }
        }
        
        List<Taxi> sortedHibasTaxi = hibasTaxis.OrderBy(t=>t.taxiIndulas).ToList();
        
        sw.WriteLine("taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja");
        foreach (Taxi taxi in sortedHibasTaxi)
        {
            sw.WriteLine(taxi.ToCSV());
        }
    }   
}

class Taxi
{
    public string taxiId { get; set; }
    public DateTime taxiIndulas { get; set; }
    public int taxiIdotartam { get; set; }
    public double taxiTavolsag { get; set; }
    public double taxiViteldij { get; set; }
    public double taxiBorravalo { get; set; }
    public string taxiFizetesModja { get; set; }
    
    public Taxi(
        string taxiId, 
        DateTime taxiIndulas, 
        int taxiIdotartam, 
        double taxiTavolsag,
        double taxiViteldij,
        double taxiBorravalo,
        string taxiFizetesModja
        )
    {
        this.taxiId = taxiId;
        this.taxiIndulas = taxiIndulas;
        this.taxiIdotartam = taxiIdotartam;
        this.taxiTavolsag = taxiTavolsag;
        this.taxiViteldij = taxiViteldij;
        this.taxiBorravalo = taxiBorravalo;
        this.taxiFizetesModja = taxiFizetesModja;
    }

    public string ToCSV()
    {
        return $"{taxiId};{taxiIndulas};{taxiIdotartam};{taxiTavolsag};{taxiViteldij};{taxiBorravalo};{taxiFizetesModja}";
    }
}