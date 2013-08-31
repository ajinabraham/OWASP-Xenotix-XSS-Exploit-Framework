from mechanize import Request, urlopen, URLError, HTTPError,ProxyHandler, build_opener, install_opener, Browser
import re
import os
import random
import threading
import Queue
import time
import string
import sys
import hashlib
import os
from optparse import OptionParser
from urlparse import urlparse,parse_qs
from urllib import urlencode
try:
    os.remove("res.txt")
except:
    pass
f=open("res.txt","a")
os.system("TITLE Xenotix XSS Exploit Framework Console")
SOURCES_RE = re.compile("""/(location\s*[\[.])|([.\[]\s*["']?\s*(arguments|dialogArguments|innerHTML|write(ln)?|open(Dialog)?|showModalDialog|cookie|URL|documentURI|baseURI|referrer|name|opener|parent|top|content|self|frames)\W)|(localStorage|sessionStorage|Database)/""")
SINKS_RE = re.compile("""/((src|href|data|location|code|value|action)\s*["'\]]*\s*\+?\s*=)|((replace|assign|navigate|getResponseHeader|open(Dialog)?|showModalDialog|eval|evaluate|execCommand|execScript|setTimeout|setInterval)\s*["'\]]*\s*\()/""")
USER_AGENTS = (
    "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)",
    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.27 (KHTML, like Gecko) Chrome/12.0.712.0 Safari/534.27",
    "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
    "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)",
    "Opera/9.80 (Windows NT 6.1; U; en) Presto/2.7.62 Version/11.01",
    "Mozilla/5.0 (X11; U; Linux i686; en-US) AppleWebKit/534.15 (KHTML, like Gecko) Ubuntu/10.10 Chromium/10.0.613.0 Chrome/10.0.613.0 Safari/534.15",
    "Mozilla/5.0 (X11; Linux i686; rv:6.0a2) Gecko/20110621 Firefox/6.0a2",
    "Opera/9.80 (X11; Linux x86_64; U; pl) Presto/2.7.62 Version/11.00",
    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:5.0) Gecko/20100101 Firefox/5.0"
)

class Engine:
    def __init__(self, target = None):
        self.targets = []
        if target is not None: self.targets.append(target)
        self.config = {}
        self.results = []
        # Container for js analysis
        self.javascript = []
        self.wl_javascript = []

    def _getTargetsQueue(self):
        queue = Queue.Queue()
        for t in self.targets:
            queue.put(t)
        return queue

    def _compactResults(self):
        temp_results = []
        for result in self.results:
            found = False
            for temp_result in temp_results:
                if temp_result.target == result.target:
                    temp_result.merge(result)
                    found = True
                    break
            if not found:
                temp_results.append(result)

        self.results = temp_results
        return True
    
    def _compactTargets(self):
        self.targets = list(set(self.targets))
        return True

    def addOption(self, key, value):
        if key in self.config:
            del self.config[key]
        self.config[key] = value

    def getOption(self, key):
        if key in self.config:
            return self.config[key] 
        else:
            return None

    def printResults(self):
        if self.getOption("dom") and len(self.javascript) == 0:
            print "[+] " + "Scan Result:" + " No DOM XSS Found "
            f.write("[+] " + "Scan Result:" + " No DOM XSS Found \n")
        elif self.getOption("dom"):
            print "[+] " + "Scan Result:" + " Possible DOM XSS in " + "%s" % len(self.javascript) + " javascripts"
            f.write("[+] " + "Scan Result:" + " Possible DOM XSS in " + "%s\n" % len(self.javascript) + " javascripts")
        
            for js in self.javascript:
                js.printResult()
        f.close()
        os.system(exit)

    def _scanDOMTargets(self):
        print "[+] DOM Scanner Started"
        f.write("[+] DOM Scanner Started\n")

        threads = []
        queue = self._getTargetsQueue()
        for i in range(min(self.getOption('threads'), len(self.targets))):
            t = DOMScanner(self, queue)
            t.setDaemon(True)
            threads.append(t)
            t.start()
      
        # Little hack to kill threads on SIGINT
        while True:
            try:
                if queue.empty() is True:
                    break
                sys.stderr.write("\r |- Remaining urls: %s " % queue.qsize())
                sys.stderr.flush()
            except KeyboardInterrupt:
                print "\r |- INTERRUPT!" + " Killing threads..."
                queue = Queue.Queue()
                break

        queue.join()
        
        # Harvest results
      
        javascript = []
        errors = {}
        for t in threads:
            for r in t.javascript:
                javascript.append(r)
           
            # errors
            for ek, ev in t.errors.iteritems():
                if errors.has_key(ek):
                    errors[ek] += ev
                else:
                    errors[ek] = ev

        # Add results to engine
        for r in javascript:
            if len(r.sources) > 0 | len(r.sinks) > 0:
                self.javascript.append(r)
      

        if errors:
            print " |--[+] " + "SCAN ERRORS!"
            f.write(" |--[+] " + "SCAN ERRORS!\n")
            for ek, ev in errors.iteritems():
                print " |   |- %sx: %s" % (len(ev), ek)
                f.write(" |   |- %sx: %s\n" % (len(ev), ek))
       

    def start(self):         
        start = time.time()
        if self.getOption('dom'):
            self._scanDOMTargets()

        print " |- Scan completed in %s seconds." % (time.time() - start)
        f.write(" |- Scan completed in %s seconds.\n" % (time.time() - start))
        print "\n[+] Processing results"
        f.write("\n[+] Processing results\n")
        self._compactResults()
        print " |- Scan Finished."
        f.write(" |- Scan Finished.\n")

        self.printResults()
        
        return True
        
class Target:
    def __init__(self, raw_url):
   
        self.rawurl = raw_url
        self.scheme = urlparse(raw_url).scheme
        self.netloc = urlparse(raw_url).netloc
        self.path = urlparse(raw_url).path
        self.method = 'GET'
        self.params = parse_qs(urlparse(raw_url).query, True)
    def getAbsoluteUrl(self):
        return self.getBaseUrl() + self.path
    def getBaseUrl(self):
        url = self.scheme if self.scheme != "" else "http"
        url += "://" + self.netloc
        return url

    def getFullUrl(self, clean=False):
        if clean:
            temp_params = {}
            for k, v in self.params.iteritems():
                temp_params[k] = ""
            return self.getAbsoluteUrl() + urlencode(temp_params)
        else:
            return self.getAbsoluteUrl() + urlencode(self.params)

class Javascript:
    def __init__(self, link, body, js_hash=None, is_embedded=False):
        self.link = link
        self.body = body
        self.is_embedded = is_embedded
        
        # javascript fingerprinting
        self.js_hash = js_hash
        if self.js_hash is None:
            self.js_hash = hashlib.md5(self.body).hexdigest()

        self.sources = []
        self.sinks = []

    def addSource(self, line, pattern):
        s = (line, pattern)
        self.sources.append(s)

    def addSink(self, line, pattern):
        s = (line, pattern)
        self.sinks.append(s)
    
    def printResult(self):
        if len(self.sources) > 0 | len(self.sinks) > 0:
            print " |--[!] Javascript: %s" % self.link
            f.write(" |--[!] Javascript: %s\n" % self.link)
            if self.is_embedded:
                print " |   |- Type: embedded"
                f.write(" |   |- Type: embedded\n")
            print " |   |--[+] # Possible Sources: " + "%s" % len(self.sources)
            f.write(" |   |--[+] # Possible Sources: " + "%s\n" % len(self.sources))
            for s in self.sources:
                print " |   |   |--[Line: %s] %s" % (s[0], s[1])
                f.write(" |   |   |--[Line: %s] %s\n" % (s[0], s[1]))
            print " |   |"
            f.write(" |   |\n")
            print " |   |--[+] # Possible Sinks: " + "%s" % len(self.sinks)
            f.write(" |   |--[+] # Possible Sinks: " + "%s\n" % len(self.sinks))
            for s in self.sinks:
                print " |   |   |--[Line: %s] %s" % (s[0], s[1])
                f.write(" |   |   |--[Line: %s] %s\n" % (s[0], s[1]))
            print " |   |"
            f.write(" |   |\n")

class DOMScanner(threading.Thread):
    def __init__(self, engine, queue):
        threading.Thread.__init__(self)
        self.queue = queue
        self.engine = engine

        self.errors = {}
        self.results = []
        self.javascript = []
        
        self.whitelist = []

        self.browser = Browser()
        self._setProxies()
        self._setHeaders()
  

    def _setHeaders(self):
        if self.engine.getOption('ua') is not None:
            if self.engine.getOption('ua') is "RANDOM":
                self.browser.addheaders = [('User-Agent', random.choice(USER_AGENTS))]
            else:
                self.browser.addheaders = [('User-Agent', self.engine.getOption('ua'))]
        if self.engine.getOption("cookie") is not None:
            self.browser.addheaders = [("Cookie", self.engine.getOption("cookie"))]
    
    def _setProxies(self):
         if self.engine.getOption('http-proxy') is not None:
            self.browser.set_proxies({'http': self.engine.getOption('http-proxy')})

    def _addError(self, key, value):
        if self.errors.has_key(key):
            self.errors[key].append(value)
        else:
            self.errors[key] = [value]


        
    def _parseJavascript(self, target):
        if self.engine.getOption("ua") is "RANDOM": self._setHeaders() 
        
        url = target.getFullUrl()
        
        try:
            to = 10 if self.engine.getOption('http-proxy') is None else 20
            response = self.browser.open(url, timeout=to) #urlopen(req, timeout=to)
            
        except HTTPError, e:
            self._addError(e.code, target.getAbsoluteUrl())
            return
        except URLError, e:
            self._addError(e.reason, target.getAbsoluteUrl())
            return
        except:
            self._addError('Unknown', target.getAbsoluteUrl())
            return
        else:
            embedded = []
            linked = []
            # Parse the page for embedded javascript 
            response = response.read()
            index = 0
            intag = False
            inscript = False
            insrc = False
            ek = 0
            lk = 0

            while index <= len(response)-1:
                if response[index:index+7].lower() == "<script":
                    intag = True
                    index += 7
                    continue
                if response[index:index+4].lower() == "src=" and intag:
                    insrc = True
                    linked.append("")
                    index += 4
                    continue
                if (response[index] == "\"" or response[index] == "'") and insrc:
                    index += 1
                    continue
                if (response[index] == "\" " or 
                    response[index] == "' ") and insrc:
                    insrc = False
                    lk += 1
                    index += 2
                    continue
                if (response[index] == "\">" or
                    response[index] == "'>") and insrc and intag:
                    insrc = False
                    intag = False
                    lk += 1
                    index += 2
                    continue
                if response[index] == " " and insrc:
                    insrc = False
                    lk += 1
                    index += 1
                    continue
                if response[index] == ">" and insrc and intag:
                    insrc = False
                    intag = False
                    inscript = True
                    embedded.append("")
                    lk += 1
                    index += 1
                    continue
                if response[index] == ">" and intag:
                    intag = False
                    inscript = True
                    embedded.append("")
                    index += 1
                    continue
                if response[index:index+9].lower() ==  "</script>" and inscript:
                    inscript = False
                    ek += 1
                    index += 9
                    continue
                if inscript:
                    embedded[ek] += response[index]
                if insrc:
                    linked[lk] += response[index]

                index += 1

            # Parse the linked javascripts
            new_linked = []
            for link in linked:
                if link == "": continue
                if link[0:len(target.getBaseUrl())] == target.getBaseUrl():
                    new_linked.append(link)
                    continue
                elif (link[0:7] == "http://" or 
                     link[0:4] == "www." or
                     link[0:8] == "https://" or
                     link[0:2] == "//"):
                    if link[0:2] == "//":
                        link = "http:" + link
                    new_linked.append(link)
                    continue
                elif link[0] == "/":
                    new_linked.append(target.getBaseUrl() + link)
                    continue
                else:
                    new_linked.append(target.getBaseUrl() + "/" + link)
             
            # Remove duplicates
            linked = list(set(new_linked))
            
            # Return all javascript retrieved
            # javascript = [ [target, content], ... ]
             
            for link in linked:
                try:
                    to = 10 if self.engine.getOption('http-proxy') is None else 20
                    response = self.browser.open(link, timeout=to)
                except HTTPError, e:
                    self._addError(e.code, link)
                    continue
                except URLError, e:
                    self._addError(e.reason, link)
                    continue
                except:
                    self._addError('Unknown', link)
                    continue
                else:
                    j = Javascript(link, response.read())
                    self.javascript.append(j)
                    
            for r in embedded:
                if r is not "": 
                    j = Javascript(target.getAbsoluteUrl(), r, True)
                    self.javascript.append(j)
     
    def _analyzeJavascript(self):
         for js in self.javascript:
             #print "\n[+] Analyzing:\t %s" % js.link

             # Check if the javascript is whitelisted
             # and eventually skip the analysis

             for k, line in enumerate(js.body.split("\n")):
                for pattern in re.finditer(SOURCES_RE, line):
                    for grp in pattern.groups():
                        if grp is None: continue
                        js.addSource(k, grp) 
                        print "[Line: %s] Possible Source: %s" % (k, grp)
                for pattern in re.finditer(SINKS_RE, line):
                    for grp in pattern.groups():
                        if grp is None: continue
                        js.addSink(k, grp) 
                        print "[Line: %s] Possible Sink: %s" % (k, grp)

    def run(self):
        """ Main code of the thread """
        while True:
            try:
                target = self.queue.get(timeout = 1)
            except:
                try:
                    self.queue.task_done()
                except ValueError:
                    pass
            else:
                
                self._parseJavascript(target)
                self._analyzeJavascript()
                
                # Scan complete
                try:                
                    self.queue.task_done()
                except ValueError:
                    pass

def main():
 
    usage = "usage: %prog [options]"

    parser = OptionParser(usage=usage)
    parser.add_option("-u", "--url", dest="url")
    parser.add_option("--threads", dest="threads", default=1)
    parser.add_option("--http-proxy", dest="http_proxy")
    parser.add_option("--user-agent", dest="user_agent")
    parser.add_option("--random-agent", dest="random_agent", default=False, 
                      action="store_true")
    parser.add_option("--cookie", dest="cookie")
    parser.add_option("--dom", dest="dom", default=False, action="store_true")

    (options, args) = parser.parse_args()
    if options.url is None: 
            x=1
            while(x==1):
                pass

    # Build a first target
    print "[+] TARGET %s" % options.url
    t = Target(options.url)

    # Build a scanner
    s = Engine(t)

    # Lets parse options for some proxy setting
    if  options.http_proxy is not None:
        s.addOption("http-proxy", options.http_proxy)
        print " |- PROXY: %s" % options.http_proxy
        f.write(" |- PROXY: %s\n" % options.http_proxy)
     # User Agent option provided?
    if options.user_agent is not None and options.random_agent is True:
        print('No --user-agent and --random-agent together!', ' |- ')
        f.write('No --user-agent and --random-agent together!\n', ' |- ')
    elif options.random_agent is False and options.user_agent is not None:
        s.addOption("ua", options.user_agent)
        print " |- USER-AGENT: %s" % options.user_agent
        f.write(" |- USER-AGENT: %s\n" % options.user_agent)
    elif options.random_agent is True:
        s.addOption("ua", "RANDOM")
        print " |- USER-AGENT: RANDOM"
        f.write(" |- USER-AGENT: RANDOM\n")

    # Cookies?
    if options.cookie is not None:
        s.addOption("cookie", options.cookie)
        print " |- COOKIE: %s" % options.cookie
        f.write(" |- COOKIE: %s\n" % options.cookie)

      # Dom scan?
    if options.dom is True:
        s.addOption("dom", True)

    # How many threads?
    s.addOption("threads", int(options.threads))

    # Start the scanning
    s.start()

if __name__ == '__main__':
    print "Xenotix XSS Exploit Framework: Basic Heuristics DOM XSS Scanner Module"
    main()
