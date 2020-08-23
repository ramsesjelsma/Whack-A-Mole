Hi!

Documentation in English.

The set-up for the scene is that everything that is GLOBAL is in the Hierarchy, and everything that is SCENE-SPECIFIC is in ScriptableObjects. 
This reduces scene management overhead and allows for easy adding and editing of level-specific content. This division of roles also helps with references - ScriptableObject's only know about things that occur outside of the scene hierarchy, and as such don't need to be set up if, theoretically, a test-scene is to be created.

Code is commented where necessary.

Play around with how easy it is to customize levels using the ScriptableObject system and have fun!