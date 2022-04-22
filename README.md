# Coding-Challenge

Escrowtab Services Code Challenge

Welcome to the Escrowtab Services Challenge, thank you for taking the time to participate.

Important:
* Please limit yourself to 1 - 2 hours of effort. Consider 2 hours an absolute maximum time limit.
* Please create a branch, perform your work there, push to github and do not merge it.
    * We download and attempt to run your branch as part of the code review process.
* We do not expect or require you to finish this challenge.

The Challenge
A new frontend application is being built to spec by a crack team of coders out in orbit around Eros. Hence they are rarely available and unable to negotiate any changes to an API specified below. The application renders and stores a tree of data. Animals, an example of a tree;
1: root
    2: ant
    3: bear
        4: cat
        5: dog
            6: elephant
    7: frog
The format is a simple unique numeric id and alphanumeric label eg, id: label. Indentation indicates a child relationship. So, 1: root has the children 2: ant, 3: bear, 7: frog. Clone this repo, create a branch, follow engineering best practices. Consider tests for any functions you write, and ensure that any requests made on the api's are valid and maintain the integrity of the data.
Spend between one to two hours in advance of the in person interview. Remaining tasks will form the basis of the in person code challenge, where you will pair with engineers from Escrowtab. Email us when pushed to let us know your work is ready for review. 1, 5, 6, 2, 3, 4 Note: we do not expect or require you to complete all 6 tasks either in advance or in person in order to receive an offer. We expect you to use this as an opportunity to demonstrate good design decisions, coding best practices, comprehension, and the ability to consider edge-cases.

API Details and Challenge Tasks
For the first four tasks, persisting data in memory or simple files is adequate. The service should run on, https://localhost:3001/api/<end-point> the following end points should be available.


1. GET /api/tree return the entire tree - in the following format;
[
    {
        "<id>": {
            "label": "<first item>",
            "children": [
                {
                    "<id>": {
                        "label": "<another item>",
                        "children": [] // empty children
                    }
                },
                {
                    "<id>": {
                        "label": "<another item>",
                        "children": [ ...<any children>... ]
                    }
                }
            ]
        }
    }
]
Task 1; Add the route and return the data structure that represents the animals example above.


2. POST /api/tree/ with the payload of the format
{
    "parent": "<id>",
    "label": "<label>"
}
Will cause a node to be added to the end of a list of children, a new unique id should be assigned by the service. The response should be simple. Task 2; Implement the route, and ensure that a GET /api/tree request returns the updated tree.


3. DELETE /api/tree/<id>
Delete the node of the corresponding id from the tree. If the node has children, the service should decline the request. Task 3; Implement the route, and ensure that a GET /api/tree request returns the updated tree.


4. UPDATE /api/tree/<id> with the following payload
{
    "current-id": <new-parent-id>
}
Causes the parent id of a node to change. The node should be appended to the list of children. Task 4; Implement the route, and ensure that a GET /api/tree request returns the updated tree.


5. Data persistance
Now persist this data after service restarts in some database of choice. Task 5; Design a data schema for a database or ORM of your choice that would support the tree data above. This does not have to be implemented in code. Add to a database.md file. Task 6; Write sample queries / code that would support the four routes that are detailed above. Add to database.md.
--
