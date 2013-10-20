#include <stdio.h>
//#include <stdlib.h>
#include <string.h>
#include <unistd.h>
//#include <stdarg.h>
//#include <errno.h>
//#include <fcntl.h>
//#include <pthread.h>
//#include <sys/types.h>
//#include <sys/stat.h>
//#include <net/if.h>
#include <arpa/inet.h>
//#include <sys/socket.h>
//#include <sys/ioctl.h>
//#include <sys/select.h>
//#include <sys/time.h>
#include <netdb.h>

#define BUFFER_SIZE		500

int createSocket(char *host, int port, struct sockaddr_in *addr)
{
	struct hostent *hp = NULL; /* remote host info from gethostbyname() */

	memset(addr, 0, sizeof(*addr));

	addr->sin_family = AF_INET;
	addr->sin_port = htons(port);
	/* If internet "a.d.c.d" address is specified, use inet_addr()
	 * to convert it into real address. If host name is specified,
	 * use gethostbyname() to resolve its address */
	addr->sin_addr.s_addr = inet_addr(host); /* If "a.b.c.d" addr */
	if (addr->sin_addr.s_addr == -1)
	{
		hp = gethostbyname(host); //hostname in form of text
		if (hp == NULL) {
			fprintf(stderr, "Host name %s not found\n", host);
			return -1;
		}
		memcpy((char *)&(addr->sin_addr.s_addr), (char *)hp->h_addr, hp->h_length);
	}


	int sockFD = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sockFD == -1)
	{
		perror("Failed to create socket.\n");
		return -1;
	}

	int optval = 1;
	/* avoid EADDRINUSE error on bind() */
	if (setsockopt(sockFD, SOL_SOCKET, SO_REUSEADDR, (char *)&optval, sizeof(optval)) < 0) {
		perror("Failed to set socket options.\n");
		return -1;
	}

	return sockFD;
}

int main(int argc, char **argv)
{
	struct sockaddr_in addr; /* remote internet address */
	char *host = "192.168.11.101";
	int sockFD;
	char buffer[BUFFER_SIZE];
	char msgBuffer[BUFFER_SIZE];
	if (( sockFD = createSocket(host, 80, &addr)) == -1) {
		return 5;
	}
	if (connect(sockFD, (struct sockaddr *) &addr, sizeof(addr)) < 0) {
		perror("Connection to server has failed.");
		return 6;
	}
	int msglen = snprintf(msgBuffer, BUFFER_SIZE, "GET %s HTTP/1.0\r\nHost: %s\r\n\r\n", "/code/by.php?write", host);
	if (write(sockFD, msgBuffer, msglen) > 0) 
	{
		int bytesRead;
		while ((bytesRead = read(sockFD, buffer, BUFFER_SIZE)) > 0)
		{
			buffer[bytesRead] = '\0';
			printf("%s", buffer);
		}
	} else {
		perror("PERROR: Error while writing to socket.");
		return 1;
	}

	close(sockFD);
	char hang[2];
	scanf("%1s", hang);
	return 0;
}
