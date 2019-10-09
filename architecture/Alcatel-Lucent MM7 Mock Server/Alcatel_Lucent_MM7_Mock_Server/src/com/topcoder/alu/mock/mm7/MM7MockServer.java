/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.net.InetSocketAddress;
import org.apache.mina.core.service.IoAcceptor;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.filter.codec.ProtocolCodecFilter;
import org.apache.mina.transport.socket.nio.NioSocketAcceptor;


/**
 * The main class of the server.
 * It creates a NioSocketAcceptor (which is like a ServerSocket) and listens for incoming requests.
 * @author TCSDEVELOPER
 * @version 1.0
 */
public class MM7MockServer {

	/**
	 * The entry point for the server.
	 * The server just runs in an infinite loop listening to the port
	 * @param args The arguments for server. It should contain just 1 argument i.e. the port number onto which to bind
	 */
	public static void main(String[] args) {
		//Argument parsing and checks
		int PORT = 0;
		if (args.length != 1) {
			System.out.println("Must contain exactly one argument (port number)");
			return;
		}
		try {
			PORT = Integer.parseInt(args[0]);
			if (PORT <= 1024) {
				System.out.println("Port number must be an integer greater than 1024");
				return;
			}
		} catch (NumberFormatException nfe) {
			System.out.println("Port number must be an integer.");
			return;
		}
		
		IoAcceptor acceptor = null;
		try {
			acceptor = new NioSocketAcceptor();
	        acceptor.getFilterChain().addLast("codec", new ProtocolCodecFilter(new SoapProtocolCodecFactory()));
	        acceptor.setHandler(new MM7MockServerHandler());
	        acceptor.bind(new InetSocketAddress(PORT));
  
	        acceptor.getSessionConfig().setReadBufferSize( 2048 );
	        acceptor.getSessionConfig().setIdleTime( IdleStatus.BOTH_IDLE, 10 );

	        System.out.println("Server now listening on port " + PORT);
		}
		catch (Exception e) {
			e.printStackTrace();
		}
		finally {
		}
	}
}
