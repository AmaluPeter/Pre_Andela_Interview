# Pre_Andela_Interview
My solution to the tasks shared InMail for the Andela opportunity. Answers to the questions are in the README, while the actual code is in the program.cs file 
## Answers
1.
	* I got the list of events using linq to query the event object. 
	* I proceeded to call the AddToEmail in a foreach loop. 
	* The expected output for John Smith from New York is
			John Smith: Phantom of the Opera in New York
			John Smith: Metallica in New York
			John Smith: LadyGaGa in New York
	* The Big O is O(n). I do not see anyway to speed it up.
2.
	* Use a ling query to get all distinct cities that arent't the customer's city, then run a loop to store all distinct city distances in a dictionary
	* After storing the cities, sort them by distance. Then select the 5 events closest to the client and call AddToEmail on each of them.
	* The expected output for John Smith from New York is
			John Smith: LadyGaGa in Chicago (221 miles away)
			John Smith: LadyGaGa in Washington (347 miles away)
			John Smith: Metallica in Boston (354 miles away)
			John Smith: LadyGaGa in Boston (354 miles away)
			John Smith: Metallica in Los Angeles (382 miles away)
	* Yes. I run 3 loops within the code. I believe using a link join on the sorted cities and the events could eliminate one loop.
3.	I would have a background job which runs and stores the distance between every city beforehand. Such that when the GetDistance is required, my data storage would 		be queried instead of the API. This would prove cost effective in the long run since the API call would only ever occur once for any two cities.
4.	Putting the code block in a try catch would solve this. Also since the background job exists, it would keep retrying the GetDistance for each city.
5.	I would pass an argument for Sort Parameter. 

### Note
Question 1 is implemented in GetEventsInCity(), while questions 2 & 6 are implemented together in GetEventsByParameter(), while questions 3 & 4 are implemented together in GetEventsJob()
