-------------------------------------------------------------------------------

1. Open up console in the <MyClientN> debug folder (Executable: Client > MyClientN > MyClientN > bin > debug > location.exe)
2. Start up the server under the <respond> debug folder (Executable: Server > respond > respond > bin > debug > locationserver.exe)
3. Call commands via the client console by writing "location" then the command you want to execute.

-------------------------------------------------------------------------------

Console commands available:

> Commands specified in <...> are parameters and should be used without <...>
> Commands specified as <...>/<...> are which parameters are available to the user, but one from the options have to be used

- location name location [adds/updates location for the name specified]
- location name [retrieves the location for the name specified]
- location <HEADER> name location [you can use the HEADERS -h0,-h9,-h1 for each respective responses to add/update the location for the name specified]
- location <HEADER> name [you can use the <HEADER> -h0,-h9,-h1 for each respective responses to get the location for the name specified]
- location GET /name [gets the name's location in 0.9 protocol]
- location GET /name <HTTP/1.0>/<HTTP/1.1> [gets the name's location in either 1.0 or 1.1 protocol]
- location PUT /name locaiton [adds/updates location in 0.9 protocol]
- location POST /name <HTTP/1.0>/<HTTP/1.1> location [adds/updates the location in 1.0 or 1.1 protocol]

-------------------------------------------------------------------------------

Client and server logs are stored in their respectable debug folders

Client log - Client > MyClientN > MyClientN > bin > debug > clientlog.txt
Server log - Server > respond > respond > bin > debug > serverlog.txt

Stored in the format: [h:mm:ss tt] [Server/Client]: TEXT