using Soneta.Core.StrukturyOrganizacyjne;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnovaGeneratorPracownikow
{
    public partial class GeneratorNazwisk
    {

        public (string plec, string imie, string nazwisko) LosujOsobe()
        {
            Random r = new Random();

            int plecInt = r.Next(0, 1);

            string plec = plecInt == 0 ? "K" : "M";

            string imie = "";
            string nazwisko = "";

            if (plec == "K")
            {

                imie = imionaZenskie[r.Next(0, imionaZenskie.Count - 1)];
                nazwisko = nazwiskaZenskie[r.Next(0, nazwiskaZenskie.Count-1)];

            }
            else
            {
                imie = imionaMeskie[r.Next(0, imionaMeskie.Count - 1)];
                

                
                nazwisko = nazwiskaMeskie[r.Next(0, nazwiskaMeskie.Count - 1)];

            }

            return (plec, imie, nazwisko);
        }



















    }
}
