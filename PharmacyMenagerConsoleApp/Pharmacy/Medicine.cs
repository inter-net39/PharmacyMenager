using System;

namespace Pharmacy
{
    public class Medicine : ActiveRecord
    {
        public Medicine()
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