# [Attack On Quack] : [Kenny, Jay, Daniel, Aaron, Adi]
# Members
Project Manager: [Ken Nguyen] ([kenny.9369])\
Communications Lead: [Jakyran Pierson] ([jakyran])\
Git Master: [Daniel Guo] ([Zernullo])\
Design Lead: [Aaron Zuvich] ([Zolvifox])\
Quality Assurance Tester: [Adithya Anand] ([_adi])

# About Our Software

Duck Defense is a 2D tower defense game where players strategically place duck turrets—including four core types (Basic, AOE, Laser, and Sniper) and three unlockable variants (Gatling Gun, Freeze, and Nuke)—to stop waves of snakes. The snakes feature six types: three basic (default, high-HP, and fast), and three bosses (Regeneration, Resurrection, and Emperor, which spawns minions). Players start with $500 currency and three lives, battling across three increasingly difficult maps. A Nuke ability wipes all on-screen enemies, while a stretch-feature Slots minigame offers optional risk-reward gameplay, letting players gamble for bonuses. The game includes music, sound effects, and upgrade paths for each duck.
## Platforms Tested on
- Windows
# Important Links
Kanban Board: [[link](https://attackonquack.atlassian.net/jira/software/projects/BTS/boards/1?cloudId=522abf96-cf51-46df-b223-57521ce60e7d&atlOrigin=eyJwIjoiaiIsImkiOiI2MjJmMmUzNDQ2Nzc0NzM2YjQ2MDkyMTQ5ZWVmOTA0YyJ9)]\
Designs: [[link](https://docs.krita.org/en/user_manual/getting_started/installation.html)]\
Styles Guide(s): [[link](https://google.github.io/styleguide/)]

# How to Run Dev and Test Environment

## Dependencies
- Unity - Unity 6 (6000.0.37f1) 
- Krita - Krita (5.2.9) 
- VsCode - version 1.99 
- Discord - Windows 11 64-bit (10.0.26100)
- .Net - SDK 9.0.203

### Downloading Dependencies
Unity
- Version: Unity 6 (6000.0.37f1)
- Download: Unity Hub (Web Download) [[download](https://unity.com/download)] (Install via Unity Hub, then select the correct version)
- Notes:
  - Requires Unity Hub for installation.
  - Next when Unity Hub is installed by clicking the install tab on the left side of the screen then click on the blue button called "install Editor" find version Unity 6 (6000.0.37f1) and install it
  - If version 6000.0.37f1 is unavailable, check Unity Beta Downloads.
  - A powerful and widely-used real-time development platform primarily known as a game engine for creating 2D, 3D, virtual reality (VR), and augmented reality (AR) experiences.
  - It provides a comprehensive suite of tools and services that enable developers and creators to build interactive content, deploy it across a multitude of platforms (including PC, consoles, mobile, and web), and manage their projects from conception to completion.
 
Krita
- Version: Krita 5.2.9
- Download: Krita Official Site (Web Download) [[download](https://krita.org/en/download/)]
- Notes:
  - Available for Windows (64-bit).
  - Portable version also offered.
  - A free and open-source digital painting program designed for artists of all levels, from beginners to professionals.
  - It offers a comprehensive suite of tools for creating illustrations, concept art, comics, 2D animations, and more, with a strong focus on providing an intuitive and customizable painting experience.

VS Code
- Version: 1.99
- Download: Visual Studio Code (Web Download) [[download](https://code.visualstudio.com/download)]
- Notes:
  - Works with Windows 10/11 (x64).
  - Extensions C# Dev Kit
  - A free and powerful source code editor developed by Microsoft that runs on Windows, macOS, and Linux.
  - It's designed to be lightweight yet highly extensible, offering features like syntax highlighting, intelligent code completion (IntelliSense), debugging tools, and built-in Git integration for version control.
 
Discord
- Version: Windows 11 64-bit (10.0.26100)
- Download: Microsoft Store [[download](https://apps.microsoft.com/detail/xpdc2rh70k22mn?launch=true&mode=full&hl=en-us&gl=us&ocid=bingwebsearch)]
- Notes:
  - Auto-updates via Microsoft Store.
  - A popular free communication platform where people can connect with friends, creators, and communities that share similar interests.
  - Users interact on servers, which are invite-only spaces that can be customized with various text and voice channels for discussing specific topics, sharing media, and hanging out in real-time.
 
.Net
- Version: SDK 9.0.203
- Download: .Net (Web Download) [[download](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)]
- Notes:
  - A free, open-source development platform created by Microsoft for building many kinds of applications, such as web, mobile, desktop, and games.
  - It supports multiple programming languages (like C#, F#, and Visual Basic) and allows developers to create applications that can run on Windows, macOS, Linux, and other operating systems.


## Commands
Describe how the commands and process to launch the project on the main branch in such a way that anyone working on the project knows how to check the affects of any code they add.

Follow this [[link](https://github.com/CSC-3380-Spring-2025/Team-14)], this link will redirect you to Team 14 github repository, by default the tab should be on "Code" and branch should be main. Find the green button name "<> Code" on the screen. Click on that button and a dropdown should pop up, below that dropdown you should see "Download Zip" option. Click on that option and once the file finsih downloading make sure to un-zip the file or an easier way is to cut (ctrl x) the file inside the zip file and paste it outside the zip file. Make sure the folder name of the file in inside the zip file be Team-14. After making sure the folder is outside the zip file open Unity if you have downloaded it already. If you don't have Unity download click this [[link](https://unity.com/download)] and download Unity. Once Unity is downloaded make sure to install version Unity 6 (6000.0.37f1) by clicking the install tab on the left side of the screen then click on the blue button called "install Editor" find version Unity 6 (6000.0.37f1) and install it. After everything is set up on the left side of Unity Hub click projects, find the "Add" button, it should be next to a blue button call "New Project." Once "add" button is click a dropdown menu should pop up, click on "Add project from disk" find Team-14 folder and go inside the Team-14 folder and another folder name "Duck TowerDefense Game" folder should be shown, click that folder and open it by the bottom right of the file explorer. This should show a project on Unity, open that file and you can start the game.

```sh
Example terminal command syntax
```

It is very common in these sections to see code in peculiar boxes to help them stand out. Check the markdown section of the Project Specifications to see how to add more / customize these.

```c#
//This regens hp for the Regeneration boss at set seconds
//--------------------------------------------------------------------
    void RegenerateHealth() {
        if (health <= 0) return; 

        regenTimer += Time.deltaTime;

        // Check if the regeneration timer has reached the interval
        // If so, regenerate health and reset the timer
        // Also ensure health does not exceed the starting health
        if (regenTimer >= regenInterval) {
            regenTimer = 0f;
            health += regenRate;

            if (health > startHealth) 
                health = startHealth;

            if (healthBar != null)
                healthBar.fillAmount = health / startHealth; // Use startHealth here as well
        }
    }
//--------------------------------------------------------------------
```
```c#
//Applies damage and slow effects on the target when using the laser--------------------------------------------------------------------
// uses LineRenderer to connect the turret to the target
    void Laser() {
        if (targetEnemy == null || targetEnemy.IsDestroyed) return; // Ensure the target is valid and not destroyed

        targetEnemy.TakeDamage(DamageOverTime * Time.deltaTime); // Apply damage over time
        targetEnemy.Slow(slowAmount); // Apply slow effect

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }
//--------------------------------------------------------------------
```
```c#
//Places the turret on the specific node
//deducts money and places the turret on the chosen spot
//--------------------------------------------------------------------
    public void PlaceTurretOn(Node node)
    {
        if (PlayerStats.Money < turretBuilding.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }
        PlayerStats.Money -= turretBuilding.cost;
        // Apply position AND rotation offset from the Node
        Quaternion rotation = Quaternion.Euler(node.rotationOffSet);
        GameObject turret = (GameObject)Instantiate(turretBuilding.prefab, node.GetPlacePosition(), transform.rotation);
        node.turret = turret;
        Turret turretScript = turret.GetComponent<Turret>();
        if (turretScript != null)
        {
        turretScript.ShowRange(2f);
        }
        Debug.Log ("Money left after building: " + PlayerStats.Money);
        turretBuilding = null;
    }
//--------------------------------------------------------------------
```
