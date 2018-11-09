using System;

namespace Pharmacy
{
    public class Prescription : ActiveRecord
    {
        public Prescription()
        {
            new LogHandler(this);
        }
        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }
    }
}