using System;
using System.Collections.Generic;
using System.Text;

namespace TopCoder.Graph.Layout
{
    public class Node : INode
    {
        public INode Container
        {
            get { return null; }
        }

        public long Id
        {
            get { return 7; }
        }

        public IList<IPort> Ports
        {
            get { return null; }
        }

        public ILabel Label
        {
            get { return null; }
        }

        public Dimension MinimalSize
        {
            get { return null; }
        }
    }
}
