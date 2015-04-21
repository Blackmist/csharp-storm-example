using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SCP;
using Microsoft.SCP.Topology;

namespace WordCount
{
    [Active(true)]
    class Program : TopologyDescriptor
    {
        static void Main(string[] args)
        {
            Console.WriteLine("No local tests - multiple streams are not supported for local tests and will generate errors. Press any key to continue.");
            Console.ReadKey();
        }

        public ITopologyBuilder GetTopologyBuilder()
        {
            // Create a new topology named 'WordCount'
            TopologyBuilder topologyBuilder = new TopologyBuilder("WordCount");

            // Add the spout to the topology.
            // Name the component 'sentences'
            // Name the field that is emitted as 'sentence'
            topologyBuilder.SetSpout(
                "sentences",
                Spout.Get,
                new Dictionary<string, List<string>>()
            {
                {Constants.DEFAULT_STREAM_ID, new List<string>(){"sentence"}}
            },
                1);
            // Add the splitter bolt to the topology.
            // Name the component 'splitter'
            // Name the field that is emitted 'word'
            // Use suffleGrouping to distribute incoming tuples
            //   from the 'sentences' spout across instances
            //   of the splitter
            topologyBuilder.SetBolt(
                "splitter",
                Splitter.Get,
                new Dictionary<string, List<string>>()
            {
                {Constants.DEFAULT_STREAM_ID, new List<string>(){"word"}}, // a default stream with a named tuple field
                {"cowbells", new List<string>(){"cowbell"}} // a named stream with a named tuple field
            },
                1).shuffleGrouping("sentences");

            // Add the counter bolt to the topology.
            // Name the component 'counter'
            // Name the fields that are emitted 'word' and 'count'
            // Use fieldsGrouping to ensure that tuples are routed
            //   to counter instances based on the contents of field
            //   position 0 (the word). This could also have been 
            //   List<string>(){"word"}.
            //   This ensures that the word 'jumped', for example, will always
            //   go to the same instance
            topologyBuilder.SetBolt(
                "counter",
                Counter.Get,
                new Dictionary<string, List<string>>()
            {
                {Constants.DEFAULT_STREAM_ID, new List<string>(){"word", "count"}}
            },
                1).fieldsGrouping("splitter", Constants.DEFAULT_STREAM_ID, new List<int>() { 0 });

            // Add the cowbell bolt, which reads from the cowbell stream
            topologyBuilder.SetBolt(
                "cowbellbolt",
                CowBell.Get,
                new Dictionary<string, List<string>>()
                  {
                      {Constants.DEFAULT_STREAM_ID, new List<string>(){"cowbellout"}} // emit a stream so we can see the cowbell
                  },
                  1).shuffleGrouping("splitter", "cowbells"); //subscribe to the stream named 'cowbell'

            // Add topology config
            topologyBuilder.SetTopologyConfig(new Dictionary<string, string>()
        {
            {"topology.kryo.register","[\"[B\"]"}
        });

            return topologyBuilder;
        }
    }
}

