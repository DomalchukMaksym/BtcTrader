# BtcTrader
The solution contains a console application and a web api application.

The app finds the best BTC trading strategy based on the amount entered, order type, and the euro and crypto balance which are the same for each exchange.

The console application accepts 5 arguments in command line argument as options:
- --btcamount \<value\> - required decimal 
- --ordertype \<value\> - required Buy/Sell
- --btcbalance \<value\> - required decimal 
- --eurobalance \<value\> - required decimal 
- --path \<value\> - optional string path to data file

Example: --btcamount 5 --ordertype Sell --btcbalance 7 --eurobalance 20 --path C:\order_books_data


Web application accepts in the body of the request same 4 required arguments, but order type is number (0 - Sell, 1 - Buy). 
Can be run as a regular visual studio project, in this case, the path to the data file can be specified via appsettings.json.
Or can be run via docker compose, in this case, the path to the data file can be specified via appsettings.json or docker compose environment variables.

Default DataPath=BtcTrader\BtcTrader.ExchangeServices\Data\order_books_data
