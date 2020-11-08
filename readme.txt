

1. Common will be used in hangfire cleint and/or server as well as in the shopify store app
2. HFServer has web api end point controller to take input from shopify store app and create jobs in hangfire
3. Now the HFServer may or may not run hangfire server
4. DbAccess is basically used by hf server for creating jobs
5. ExampleThirdPartyWorker shows how any app can just create a worked and how it would be added in their server 
   (
		when i say their server, they can have their own server or they can use HFServer as a template
		they can even use a console app and add DbAccess, Common projects and their worker project like in this solution
		ExampleThirdPartyWorker and just start the background job processor from hangfire and it will be their HF server
   )