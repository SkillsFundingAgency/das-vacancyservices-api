# Postman Scripts

## Components
1. Collection: These are the group of tests. 
2. Environment: These are the settings required for the tests.

## Run scripts using the Postman application
1. Create a **Personal** workspace (optional but recommended). 
2. Import collections that you want to execute **(*-collection.json)**.
3. Import any one environment file **(*-environment.json)** along with the collection. 
> Note: The environment file is tokenised to be able to run in Release cycle. 
4. Replace tokens in the environment settings with valid values.
> Note: Target urls are constructed using environment variables `<apim-gateway-url>/<manage-vacancies-path>/resource`, where gateway url is the base website url `https://mywebapi.com`, path is the service with environment prefix `vacancies-pre` and resources are hard coded for each test. 
5. Run the collection using the `Run Collection` wizard. 
> Note: Some tests are required to be executed sequentially as they depend on previous tests to populate some environment settings. But most tests can be executed in isolation.

## Run scripts using Newman command line
> Note: Newman is command line tool which requires Node.js. [Here](https://www.getpostman.com/docs/v6/postman/collection_runs/command_line_integration_with_newman) is the link to it's documentation.  

1. Find the environment json for the collection you want to execute and create a copy of it. 
> Note: If you prefix file name with `._` and keep the suffix as `-environment.json` then git will ignore it.  
2. Populate the settings in the json file by editing it. 
3. Open teminal/command window. 
4. Make sure that the Newman is in the path. 
5. Navigate to the folder where your collections and modified environment files are. 
6. Execute collection using following comment line. 
```
newman run <collection-file-name> -e <environment-file-name>
``` 
> Note: The tool logs verbosely and gives you a summmary at the end. Optionally you can use reporters for formatted log, see documentation page for help. 