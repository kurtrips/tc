1)
I am just submitting the source and not the tests. 
There are a few tests in MM7MockClientTests.java but I have been unable to automate their running.
I have been able to individually run them and see their output in the [xyz].log file created on the root directory and they seem to be giving the correct output.
Please note that 
a) The required deliverables on wiki only includes source code and not tests.
b) There was an extension request which was denied inspite of this being a tough contest.

2) Other info about the submission:

The submission uses Apache MINA 2.0.0 as the TCP framework (see http://mina.apache.org/)
The submission uses Apache James Mime4J 0.6 for parsing multipart http content (see http://james.apache.org/)
The submission uses JAXB RI for the ability to use proper namespace prefixes while marshalling (see https://jaxb.dev.java.net/2.2.1/)

3) You can try the following:
ant clean
ant run_server (in one command prompt)
ant run_client (in another command prompt)
Ctrl+C (on server command prompt)

You should see a server.log file which is the output of test_case1
I haven't been able to figure out so far why we need the Ctrl+C :(
