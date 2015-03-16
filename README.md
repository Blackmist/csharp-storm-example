
This is a basic C# Storm topology for Apache Storm on HDInsight clusters (on Microsoft Azure.)

This demonstrates how to create both spouts and bolts, as well as how to emit multiple streams. Here's the basic flow:

1. Random sentences are emitted by the spout

2. Each sentence is split into words by the splitter bolt

    * Each word is emitted to the default stream

    * If the word is 'cow', then a string value of `ding` is emitted to the 'cowbells' stream

3. The counter bolt consumes the words, and emits a count of how many times each word has occurred.

4. The cowbell bolt listens for cowbells and emits emits the 'ding' to it's default output stream

To build/use, follow the steps in http://azure.microsoft.com/en-us/documentation/articles/hdinsight-storm-develop-csharp-visual-studio-topology/ to learn how to build/deploy C# topologies to Storm on HDInsight.

NOTE: If you follow through the article above, there's a part about local testing. This doesn't seem to work with multiple streams, as the SCP.NET framework returns an exception complaining about the streams.
