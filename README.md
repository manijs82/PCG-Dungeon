# PCG-Dungeon

Research project on procedural dungeon generation done in unity.
In this project I am using various algorithms to generate a rough-like dungeon.

## Steps of the algorithm

1- Evolution strategy for generating rooms
![step1](https://user-images.githubusercontent.com/57400375/230924478-20ff97cc-2c19-4dea-9e35-ea9ddccb3064.png)

2- Making a graph out of the rooms with a Triangulation algorithm
With the bowyer-watson algorithm which does a Delaunay triangulation I connect the rooms in a way that it looks like paths between rooms.
![step2](https://user-images.githubusercontent.com/57400375/230924690-4fd772ca-e73c-4b05-80e6-71799fba1f91.png)

3- Tweeking the graph to a Minimum-Spanning-Tree
With the Prim's algorithm which makes a Minimum-Spanning-Tree out of the created room graph. After this step every room will only acessible one way.
I add some of the connections back so it has some variation.
![step3](https://user-images.githubusercontent.com/57400375/230924829-3094effd-a2b4-4390-8a94-62cfe7b3dccf.png)

4- Visualize
![final](https://user-images.githubusercontent.com/57400375/230924879-4f9c1de1-1f9b-4ebf-a5a8-1ee8db7efa78.png)
