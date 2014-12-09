'''
Static DOM XSS Scanner
Author : Ajin Abraham
Usage: Usage Static DOM XSS Scanner.py -d <dir>
Description: 
Static DOM XSS Scanner is a Static Analysis tool that will iterate through all
the JavaScript and HTML files under the given directory and will list out all the possible
sources and sinks that may cause DOM XSS. At the end of the scan, the tool will generate an HTML report.'''
import re,os,webbrowser
from optparse import OptionParser
def DOMAnalyzer(path):
    print "Running DOM XSS Analyzer"
    SOURCES_RE = re.compile("""/(location\s*[\[.])|([.\[]\s*["']?\s*(arguments|dialogArguments|innerHTML|write(ln)?|open(Dialog)?|showModalDialog|cookie|URL|documentURI|baseURI|referrer|name|opener|parent|top|content|self|frames)\W)|(localStorage|sessionStorage|Database)/""")
    SINKS_RE = re.compile("""/((src|href|data|location|code|value|action)\s*["'\]]*\s*\+?\s*=)|((replace|assign|navigate|getResponseHeader|open(Dialog)?|showModalDialog|eval|evaluate|execCommand|execScript|setTimeout|setInterval)\s*["'\]]*\s*\()/""")   
    DATA=""
    for dirName, subDir, files in os.walk(path):
        for jfile in files:
            jfile_path=os.path.join(path,dirName,jfile)
            if jfile.endswith('.js') or jfile.endswith('.html'):
                try:
                
                    with open(jfile_path,'r') as f:
                        dat=f.read()
                    thd="<tr><td>"+jfile+"</br>["+jfile_path+"]</td>"
                    source=""
                    sink=""
                    for k, line in enumerate(dat.split("\n")):
                        for pattern in re.finditer(SOURCES_RE, line):
                            for grp in pattern.groups():
                                if grp is None: continue
                                if (("[Line: "+str(k+1)+"]" in source) and (grp in source)):
                                    pass
                                else:
                                    source+="[Line: "+ str(k+1) +"] - "+ grp +"</br>"
                               
                        for pattern in re.finditer(SINKS_RE, line):
                            for grp in pattern.groups():
                                if grp is None: continue
                                if (("[Line: "+str(k+1)+"]" in sink) and (grp in sink)):
                                    pass
                                else:
                                    sink+="[Line: "+ str(k+1) +"] - "+ grp +"</br>"
                                
                    if (len(source) > 2 or len(sink)>2):
                        DATA+=thd+"<td>"+source+"</td><td>"+sink+"</td></tr>"
                except:
                    pass

    return DATA
                            
def Report(data):
    path = os.path.join(os.getcwd() + '/template/template.html') 
    with open(path,'r') as f:
        dat=f.read()
    report_path= os.path.join(os.getcwd() + '/Report.html')
    with open(report_path,'w') as f:
        f.write(dat.replace('{{DATA}}',data))
    print "Report generated.\nOpening Report.html"
    webbrowser.open_new_tab(report_path)
    
def main():
 
    usage = "usage: %prog -d <dir>"
    parser = OptionParser(usage=usage)
    parser.add_option("-d", "--dir", dest="dir")
    (options, args) = parser.parse_args()
    if options.dir is not None:
        Report(DOMAnalyzer(options.dir))
    else:
        print "Usage: Static DOM XSS Scanner.py -d <dir>"
       
    
        
if __name__ == '__main__':
    main()
