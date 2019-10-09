/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;


/**
 * The client for the server. It is test class of the project
 * THIS CLASS IS INCOMPLETE AS OF NOW
 * @author TCSDEVELOPER
 * @version 1.0
 */
public class MM7MockClientTests {

	/**
	 * The port number of the server
	 */
	private static int PORT;
	
	/**
	 * The entry point for the tests.
	 * The tests are just run one after another
	 * @param args The arguments for client tests. 
	 * It should contain just 1 arguments:
	 * a) The port number of the server 
	 * @throws Exception 
	 */
	public static void main(String[] args) throws Exception {
		//Argument parsing and checks
		if (args.length != 1) {
			System.out.println("Must contain exactly one integer argument");
			return;
		}
		try {
			PORT = Integer.parseInt(args[0]);
			
			if (PORT <= 1024) {
				System.out.println("Port number must be an integer greater than 1024");
				return;
			}
		} catch (NumberFormatException nfe) {
			System.out.println("Arguments must be integer");
			return;
		}
		
        //CLIENT test calls begin here
		
        /////////////////Test 1 - MM7 Submit Request/////////////////////////
        makeTestMM7SubmitRequest();
        
        /////////////////Test 2 - MM7 Delivery Report Response/////////////////////////
        makeTestMM7DeliveryReportReponse();
        
        /////////////////Test 3 - MM7 Deliver Response/////////////////////////
        makeTestMM7DeliverReponse();
        
        /////////////////Test 4 - Invalid MM7 Submit Request/////////////////////////
        makeInvalidMM7SubmitRequest1();
	}

	/**
	 * 
	 */
	private static void makeTestMM7DeliveryReportReponse() throws Exception {
        //Make client's MM7 Submit Request call
		makeTestCallFromFile("res/sample_delivery_report_response.txt");
	}

	private static void makeTestMM7SubmitRequest() throws Exception {
		makeTestCallFromFile("res/sample_submit_req_from_spec.txt");
	}
	
	private static void makeTestMM7DeliverReponse() throws Exception {
		makeTestCallFromFile("res/sample_deliver_response.txt");
	}
	
	private static void makeInvalidMM7SubmitRequest1() throws Exception {
        //Wrong Content-Type
		makeTestCallFromFile("res/sample_submit_req_invalid1.txt");
	}
	
	private static void makeTestCallFromFile(String fileName) throws Exception {
        Socket clientSock = new Socket(InetAddress.getLocalHost(), PORT);
        OutputStream os = clientSock.getOutputStream();
        
        //Read the request from file
        File file = new File(fileName);
        FileInputStream fin = new FileInputStream(file);
        byte fileContent[] = new byte[(int)file.length()];
        fin.read(fileContent);
        os.write(fileContent);
        os.flush();
        
        //Write the response to a file
        InputStream is = clientSock.getInputStream();
        OutputStream out = new FileOutputStream(new File("server.log"));
        int ch = -1;
		while ((ch = is.read()) != -1) {
        	out.write(ch);
        }
		
		out.flush();
		out.close();
		
		os.close();
		is.close();
		clientSock.close();
	}
}
