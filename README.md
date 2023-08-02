# PCG-Dungeon

Research project on procedural dungeon generation done in unity.
In this project I am using various algorithms to generate a rough-like dungeon.

## Current Output
| ![Screenshot 2023-07-31 145706](https://github.com/manijs82/PCG-Dungeon/assets/57400375/63639277-7ddb-4abd-b198-151e6b7d61d2) | 
|:--:| 
35 rooms in a 150x150 grid <br>
Each room has a width and height between 10 and 20 cells
- 4 different room types
  - Main path has its own room type
  - branches of the main path have their own type (randomized between 2 types)
  - Alone rooms have their own room type (green rooms)

## Steps of the algorithm

1- Evolution strategy for generating rooms <br>
Using a evaluation function I can give a fitness value to a completely radomly generated room placement. <br>
By mutating the best samples over and over until getting a sample that has a good score you will get a suitable sample to generate a dungeon for. <br> 
<img src="https://user-images.githubusercontent.com/57400375/230924478-20ff97cc-2c19-4dea-9e35-ea9ddccb3064.png" alt="step1" width="600"/> <br>

2- Making a graph out of the rooms with a Triangulation algorithm <br>
With the bowyer-watson algorithm which does a Delaunay triangulation I connect the rooms in a way that it looks like paths between rooms. <br>
Rooms are then connected with A* path finding. <br>
<img src="https://user-images.githubusercontent.com/57400375/230924690-4fd772ca-e73c-4b05-80e6-71799fba1f91.png" alt="step2" width="600"/> <br>

3- Tweeking the graph to a Minimum-Spanning-Tree <br>
With the Prim's algorithm which makes a Minimum-Spanning-Tree out of the created room graph. After this step every room will only acessible one way. <br>
You can add some of the connections back so it has some variation. <br>
<img src="https://user-images.githubusercontent.com/57400375/230924829-3094effd-a2b4-4390-8a94-62cfe7b3dccf.png" alt="step3" width="600"/> <br>

4- Visualize <br>
<img src="https://user-images.githubusercontent.com/57400375/230924879-4f9c1de1-1f9b-4ebf-a5a8-1ee8db7efa78.png" alt="final" width="600"/> <br>
