
import re, requests, threading
import tkinter as tk
from lxml import etree

class Naifei():
    def __init__(self):
        self.old_link = e1.get()
        self.header = {'User-Agent': 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36'}
        self.spider()

    def get_link(self):
        link_pattern = re.compile(r'(?<=https://pan.baidu.com/s/)\S+')
        try:
            link = link_pattern.findall(self.old_link)[0]
        except IndexError:
            t.insert('end', '未匹配到百度云链接\n')
            return None
        code_pattern = re.compile(r'(?<=提取码:)\w+')
        code = code_pattern.findall(self.old_link)
        if not code:
            code_pattern = re.compile(r'(?<=提取码: )\w+')
            code = code_pattern.findall(self.old_link)
            if not code:
                code_pattern = re.compile(r'(?<=提取码： )\w+')
                code = code_pattern.findall(self.old_link)
                if not code:
                    code_pattern = re.compile(r'(?<=提取码：)\w+')
                    code = code_pattern.findall(self.old_link)
                    if not code:
                        code_pattern = re.compile(r'(?<=提取码)\w+')
                        code = code_pattern.findall(self.old_link)
                        if not code:
                            t.insert('end', '未匹配到提取码\n')
                            return None
        code = code[0]
        new_link = f'http://pan.naifei.cc/?share={link}%20&pwd={code}'
        return new_link

    def spider(self):
        url = self.get_link()
        if not url:
            return
        response = requests.get(url, headers=self.header)
        if response.status_code != 200:
            t.insert('end', 'HTTP:' + response.status_code)
            return
        html = response.content.decode('utf-8')
        with open("123.html", "w", encoding="utf-8") as f:
            f.write(html)
        tree = etree.HTML(html)
        try:
            download_link = tree.xpath('/html/body/table/tbody/tr/td[3]/a/@href')[0]
        except IndexError:
            t.insert('end', f'未找到下载链接，请去{url}查看\n')
        t.insert('end', download_link + '\n\n')
        for i in range(2,20):
            try:
                download_link = tree.xpath(f'/html/body/table/tbody/tr[{i}]/td[3]/a/@href')[0]
                t.insert('end', download_link + '\n\n')
            except IndexError:
                break
        

class Application(tk.Tk):
    def __init__(self):
        super().__init__()
        global e1, t
        self.title('百度云链接解析') # 给窗口的可视化起名字
        self.geometry('600x400')  # 设定窗口的大小(长 * 宽)
        # 文字
        l1 = tk.Label(self, text='请完整复制粘贴百度云分享链接：',font=('pingfang', 12))
        l1.place(x=140,y=30,anchor='s')
        l1 = tk.Label(self, text='↓↓复制下面链接到浏览器即可开始下载↓↓',font=('pingfang', 12))
        l1.place(x=180,y=140,anchor='s')
        # 输入框
        e1 = tk.Entry(self, width=80, show=None)
        e1.place(x=300,y=60,anchor='s')
        # 文本框
        t = tk.Text(self, width=80, height=13)
        t.place(x=300,y=330,anchor='s')
        # 按钮
        b1 = tk.Button(self, text='解析', font=('pingfang', 12), width=12, height=2, command=lambda :self.thread_it(Naifei))
        b1.place(x=300,y=110,anchor='s')

    @staticmethod
    def thread_it(func):
        '''打包进线程'''
        t = threading.Thread(target=func) 
        t.setDaemon(True)
        t.start()

if __name__ == "__main__":
    Application().mainloop()