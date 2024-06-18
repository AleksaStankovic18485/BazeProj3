using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NarodnaSkupstina.Entiteti
{
    internal class BioPrisutanId
    {
        internal protected virtual NarodniPoslanik NPPrisutan { get; set; }
        internal protected virtual Sednica SednicaZasedanja { get; set; }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof(BioPrisutanId))
                return false;

            BioPrisutanId recievedObject = (BioPrisutanId)obj;

            if ((NPPrisutan.IdentifikacioniBroj == recievedObject.NPPrisutan.IdentifikacioniBroj) &&
                (SednicaZasedanja.Id == recievedObject.SednicaZasedanja.Id))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
