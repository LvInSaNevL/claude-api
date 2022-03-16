from curses import raw
from distutils.file_util import write_file
from queue import Full
import requests
import dataclasses
import json
import os

allGames = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json"
targetedGame = "http://store.steampowered.com/api/appdetails?appids="

@dataclasses.dataclass
class Game:
    Id: str
    Title: str
    About: str
    Release: str
    Developer: str
    Publisher: str
    Launcher: str 
    Thumbnail: str
    Screenshots: list
    

def main():
    os.mkdir("steamapps")
    request = requests.get(allGames)
    data = request.json()["applist"]["apps"]
    count = 0

    for game in data:
        try:
            gameRequest = requests.get(targetedGame + str(game["appid"]))
            gameData = gameRequest.json()[str(game["appid"])]["data"]

            if (gameData["type"] == "game"):
                count += 1
                print("{}: Getting info for {}, Steam ID: {}".format(count, gameData["name"], gameData["steam_appid"]))
                screens = []
                for g in gameData["screenshots"]:
                    screens.append(g["path_full"])    

                rawGame = Game(
                    Id=gameData["steam_appid"],    
                    Title=gameData["name"], 
                    About=gameData["about_the_game"],
                    Release= gameData["release_date"]["date"],       
                    Developer=gameData["developers"][0],
                    Publisher=gameData["publishers"][0],
                    Launcher="Steam",
                    Thumbnail=" ".format(gameData["steam_appid"]),
                    Screenshots=screens    
                )
                parsedGame = dataclasses.asdict(rawGame)
                with open("steamapps/{}.Steam.json".format(gameData["steam_appid"]), 'w+') as f:
                    json.dump(parsedGame, f, ensure_ascii=True)
        except:
            pass
    
def fixer():
    files = os.listdir("steamapps/")
    count = 0

    for game in files:
        parts = game.split('.')
        if (parts[1] == "Steam"):
            count += 1
            with open("steamapps/{}".format(game), 'r+') as readFile:
                gameData = json.load(readFile)

                screens = []
                try:
                    for group in gameData["Screenshots"]:
                        screens.append(group["Full"])
                except:
                    screens = gameData["Screenshots"]

                rawGame = Game(
                    Id=gameData["Id"],    
                    Title=gameData["Title"], 
                    About=gameData["About"],
                    Release= gameData["Release"],      
                    Developer=gameData["Developer"],
                    Publisher=gameData["Publisher"],
                    Launcher="Steam",
                    Thumbnail=gameData["Thumbnail"],
                    Screenshots=screens    
                )

                parsedGame = dataclasses.asdict(rawGame)
                readFile.seek(0)
                json.dump(parsedGame, readFile, ensure_ascii=True)
                readFile.truncate()
                print("Writing to file {}/{}, {}".format(count, len(files), game))



# Actual start
if __name__ == "__main__":
    fixer()