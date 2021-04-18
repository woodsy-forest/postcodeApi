# postcodeApi #

## Installation ##
Set the Connection String in appsettings.Development.json

## Tests ##

There are two end points:

1. GET: https://api.postcodes.io/postcodes/EX1 1NT

2. POST: https://api.postcodes.io/postcodes 

   HEADER: KEY: Content-Type   VALUE: application/json 

   BODY: 
   

```json
{
  "postcodes" : ["OX49 5NU", "M32 0JG", "NE30 1DP1"]
}
```
