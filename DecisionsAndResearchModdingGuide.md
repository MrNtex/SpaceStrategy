<body>
    <h1>Space Game Modding Guide</h1>
    <h2>Accessing the Resources Folder</h2>
    <h3>Introduction</h3>
    <p>This guide aims to help modders gain access to the Resources folder of the game. Specifically, we will focus on the Research and Decisions folders, where technology categories and decisions are stored as JSON files, respectively.</p>
    <h3>Resources Folder Structure</h3>
    <p>The Resources folder contains various subfolders, each serving a different purpose. For modding technology categories and decisions, the primary focus will be on the Research and Decisions folders. Below is an example structure:</p>
    <pre>
        Assets
        └── Resources
            ├── Research
            │   ├── Technology.json
            │   ├── Physics.json
            │   └── Chemistry.json
            └── Decisions
                ├── Decisions.json
        └── GFX
            └── Decisions
                ├── Decision1.png
                ├── Decision2.png
    </pre>
    <h3>Example JSON Structure for Research</h3>
    <p>Each file in the Research folder represents a technology category and follows a specific JSON structure. Here is an example file named <code>Technology.json</code>:</p>
    <pre>
        {
          "name": "Technology",
          "researches": [
            {
              "id": 0,
              "name": "AI Development",
              "description": "Research on artificial intelligence development.",
              "cost": 500,
              "prerequisites": [],
              "researched": false,
              "unlockedDate": "2024-05-10T12:00:00",
              "x": 0,
              "y": 0
            },
            {
              "id": 1,
              "name": "Quantum Computing",
              "description": "Research on quantum computing advancements.",
              "cost": 1000,
              "prerequisites": [
                0
              ],
              "researched": false,
              "unlockedDate": "2024-05-15T12:00:00",
              "x": 1,
              "y": 2.5
            },
            {
              "id": 2,
              "name": "Smth",
              "description": "Research on quantum computing advancements.",
              "cost": 1000,
              "prerequisites": [
                1
              ],
              "researched": false,
              "unlockedDate": "2024-05-15T12:00:00",
              "x": 2,
              "y": 2.5
            }
          ]
        }
    </pre>
    <h3>Example JSON Structure for Decisions</h3>
    <p>Each file in the Decisions folder represents a set of decisions and follows a specific JSON structure. Here is an example file named <code>Decisions.json</code>:</p>
    <pre>
        {
          "decisions": [
            {
              "id": 1,
              "name": "Decision1",
              "description": "Description of Decision1",
              "cost": 100,
              "next": [2, 3],
              "effectsString": ["Effect3"],
              "countriesLikingString": ["CHI: 20", "USA: 10"]
            },
            {
              "id": 2,
              "name": "Decision2",
              "description": "Description of Decision2",
              "cost": 200,
              "next": [1],
              "effectsString": ["Effect3"],
              "countriesLikingString": ["CHI: 20", "USA: 10"]
            }
          ]
        }
    </pre>
    <h3>Understanding the JSON Fields for Research</h3>
    <ul>
        <li><code>name</code>: The name of the technology category.</li>
        <li><code>researches</code>: An array of research items within this category.
            <ul>
                <li><code>id</code>: Unique identifier for the research item.</li>
                <li><code>name</code>: The name of the research item.</li>
                <li><code>description</code>: A brief description of what the research entails.</li>
                <li><code>cost</code>: The resource cost required to research this item.</li>
                <li><code>prerequisites</code>: An array of research item IDs that must be completed before this item can be researched.</li>
                <li><code>researched</code>: Boolean indicating if the research has been completed.</li>
                <li><code>unlockedDate</code>: The date and time when this research item becomes available.</li>
                <li><code>x</code>, <code>y</code>: Coordinates for the item's position in the research tree UI.</li>
            </ul>
        </li>
    </ul>
    <h3>Understanding the JSON Fields for Decisions</h3>
    <ul>
        <li><code>decisions</code>: An array of decision items.
            <ul>
                <li><code>id</code>: Unique identifier for the decision item.</li>
                <li><code>name</code>: The name of the decision item.</li>
                <li><code>description</code>: A brief description of what the decision entails.</li>
                <li><code>cost</code>: The resource cost required to make this decision.</li>
                <li><code>next</code>: An array of decision item IDs that follow this decision.</li>
                <li><code>effectsString</code>: An array of effects that result from this decision.</li>
                <li><code>countriesLikingString</code>: An array of country relationships affected by this decision, in the format "COUNTRY_CODE: VALUE".</li>
            </ul>
        </li>
    </ul>
    <h2>Modding Instructions</h2>
    <p>To add or modify technology categories or decisions in the game, follow these steps:</p>
    <ol>
        <li><strong>Locate the Research or Decisions Folder</strong>:
            <ul>
                <li>Navigate to <code>Assets/Resources/Research</code> or <code>Assets/Resources/Decisions</code> within your Unity project directory.</li>
            </ul>
        </li>
        <li><strong>Add a New JSON File</strong>:
            <ul>
                <li>Create a new JSON file for your technology category or decisions. Name it appropriately (e.g., <code>AdvancedTech.json</code> or <code>NewDecisions.json</code>).</li>
            </ul>
        </li>
        <li><strong>Edit the JSON File</strong>:
            <ul>
                <li>Open the JSON file in a text editor.</li>
                <li>Follow the provided structure to define your new research items or decisions.</li>
            </ul>
        </li>
        <li><strong>Save and Test</strong>:
            <ul>
                <li>Save your changes and ensure the JSON file is properly formatted.</li>
                <li>Load your game in Unity and check the relevant section to see if your new technology category or decisions appear and function as expected.</li>
            </ul>
        </li>
    </ol>
    <h2>Example Modding Scenario</h2>
    <p>Let's add a new technology category called "Advanced Technology" with two research items, and a new set of decisions called "Strategic Decisions" with two decision items.</p>
    <h3>AdvancedTech.json</h3>
    <pre>
        {
          "name": "Advanced Technology",
          "researches": [
            {
              "id": 0,
              "name": "Nanotechnology",
              "description": "Research on nanotechnology advancements.",
              "cost": 800,
              "prerequisites": [],
              "researched": false,
              "unlockedDate": "2024-06-01T12:00:00",
              "x": 0,
              "y": 0
            },
            {
              "id": 1,
              "name": "Biotechnology",
              "description": "Research on biotechnology developments.",
              "cost": 1200,
              "prerequisites": [
                0
              ],
              "researched": false,
              "unlockedDate": "2024-06-10T12:00:00",
              "x": 1,
              "y": 1.5
            }
          ]
        }
    </pre>
    <p>Save this file as <code>AdvancedTech.json</code> in the <code>Assets/Resources/Research</code> directory.</p>
    <h3>NewDecisions.json</h3>
    <pre>
        {
          "decisions": [
            {
              "id": 1,
              "name": "Economic Reform",
              "description": "Implement economic reforms to boost growth.",
              "cost": 300,
              "next": [2],
              "effectsString": ["Effect1"],
              "countriesLikingString": ["USA: 15", "RUS: -10"]
            },
            {
              "id": 2,
              "name": "Military Expansion",
              "description": "Expand the military to strengthen defense.",
              "cost": 500,
              "next": [1],
              "effectsString": ["Effect2"],
              "countriesLikingString": ["USA: 10", "CHN: -20"]
            }
          ]
        }
    </pre>
    <p>Save this file as <code>NewDecisions.json</code> in the <code>Assets/Resources/Decisions</code> directory.</p>
    <h3>Final Steps</h3>
    <ol>
        <li><strong>Reload Unity</strong>: After saving the new JSON files, reload your Unity project.</li>
        <li><strong>Check the Research and Decisions Menus</strong>: Open the research and decisions menus in your game and verify that "Advanced Technology" and "Strategic Decisions" appear with the new items.</li>
    </ol>
    <p>By following these steps, you can easily expand and customize the technology categories and decisions in your Unity game, providing a richer experience for players and modders alike.</p>
</body>
