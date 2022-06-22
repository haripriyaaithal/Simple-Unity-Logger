# Simple Logger
A light weight UI with which you can view & save the debug logs printed by Unity, in the build.

## Instructions to use SimpleLogger
- Place the ```SimpleLogger.prefab``` on the scene.
- To enable or disable SimpleLogger, go to menu ```Tools/SimpleLogger/(Enable or Disable)```  
  This adds a scripting define symbol ```USE_SIMPLE_LOGGER``` to your project settings.    
  
  **IMPORTANT!**  
  If the above scripting define symbol is not present in your project settings, the SimpleLogger prefab in the scene will be destroyed on ```Awake()``` & you'll not be able to access the logs UI.  
   
 - If you see some UI glitches with infinite scroll, try enabling the ```Use Target Frame Rate``` variable on ```LogManager``` script which is placed on      ```SimpleLogger``` prefab and set your target framerate.
 - To enable the logs UI, you can use the ```Show Logs``` button which is present the same prefab. Ideally it's better to move this button to your debug tool.  
   You can just enable ```Panel - Logs``` game object which is under ```SimpleLogger``` game object to enable the UI from your custom button.  
   
## Saving the logs
#### Saving logs locally
- The logs saved locally will be stored in ```Application.persistentDataPath/Logs``` folder.
#### Saving logs in GitHub Gists
- To use GitHub gists, auth token is required.
- To create personal access token go to [https://github.com/settings/tokens](https://github.com/settings/tokens)
  - Click on ```Generate New Token```
  - Select Expiration as ```No Expiration```
  - Enable ```gists``` under "Scopes define the access for personal tokens" section.
  - Click on ```Generate token``` button
  - Now copy the personal access token and paste it in ```Personal Access Token``` field which is in ```SaveUI``` script on ```Panel - Save``` game object.
- If you want to save the logs in any other places like AWS S3, Firebase Storage, PasteBin etc., you can implement ```ILogWriter``` interface and add the logic in the new script, then map the button callback to use the new script to store the logs.

# Demo
https://user-images.githubusercontent.com/16334448/175096515-aba99315-945b-4a77-b0cd-b30429a0e04a.mp4

