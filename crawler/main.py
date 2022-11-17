from bs4 import BeautifulSoup
from fake_useragent import UserAgent
import requests
import time
import random
#偽裝
ua = UserAgent()
user_agent = ua.random
headers = {'user-agent': user_agent}
web_url = "https://dictionary.cambridge.org/dictionary/english-chinese-traditional/"

#set to store already run vocabulary
readset = set()
#read vocabulary file
with open("input_1.txt", "r") as input_1, open("output_1.txt", "a+") as output_1:
    i = 0
    for v in input_1:
        
        time.sleep(random.uniform(0.0, 3.0))
        vocabulary = v.rstrip()
        print(time.ctime() + " : " + vocabulary)
        
        response = requests.get(web_url+vocabulary, headers=headers)
        response.encoding = "utf-8"
        soup = BeautifulSoup(response.content, "lxml")
        readset.add(v)
        try:
            #詞性
            i += 1
            pofs =  soup.find("span", {"class":"pos dpos"}).text
            #詞意
            meaning = soup.find("span", {"lang":"zh-Hant"}).text
            #例句
            sentence = soup.find("span", {"class":"eg deg"}).text #find().text 這樣只會拿text部分
            output_1.writelines(vocabulary + "|" + pofs + "|" + meaning + "|" + sentence + "\n")
        except:
            continue

        
        if(i >= 150):break



#clear already read vocabulary
with open("input_1.txt", "r") as input_1:
    filedata = input_1.read()
for rs in readset:
    filedata = filedata.replace(rs, "")
with open("input_1.txt", "w") as input_1:
    input_1.write(filedata)


