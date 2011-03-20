Type.registerNamespace("Ace")

// constructor
Ace.GridRowHyperlinkExtender = function (element) 
{
    Ace.GridRowHyperlinkExtender.initializeBase(this, [element]);
    
    this.TargetUrl = null;
}

Ace.GridRowHyperlinkExtender.prototype.initialize = function () 
{
    Ace.GridRowHyperlinkExtender.callBaseMethod(this, "initialize");
    
    var j = 0;
    
    for (var i = 0; i < this.get_element().rows.length; i++)
    {
        if (this.get_element().rows[i].cells[0].tagName != 'TH')
        {
            $addHandlers(this.get_element().rows[i], { 'click': this.onClick }, this);

            this.get_element().rows[i].index = j++;
        }
    }
}

Ace.GridRowHyperlinkExtender.prototype.dispose = function () 
{
    for (i=0; i < this.get_element().rows.length; i++)
    {
        $clearHandlers(this.get_element().rows[i]);
    }

    Ace.GridRowHyperlinkExtender.callBaseMethod(this, "dispose");
}

Ace.GridRowHyperlinkExtender.prototype.onClick = function (eventElement) 
{
    if(!eventElement.disabled && eventElement.target.tagName != 'TH')
    {
        var target = this.TargetUrl + this.IdList[eventElement.target.parentNode.index]
        
        window.open(target, '_self');
    }
}

Ace.GridRowHyperlinkExtender.prototype.get_TargetUrl = function ()
{
    return this.TargetUrl;
}

Ace.GridRowHyperlinkExtender.prototype.set_TargetUrl = function (value)
{
    if (this.TargetUrl != value)
    {
        this.TargetUrl = value;
        this.raisePropertyChanged("TargetUrl");
    }
}

Ace.GridRowHyperlinkExtender.prototype.get_IdList = function ()
{
    return this.IdList;
}

Ace.GridRowHyperlinkExtender.prototype.set_IdList = function (value)
{
    if (this.IdList != value)
    {
        this.IdList = value;
        this.raisePropertyChanged("IdList");
    }
}

Ace.GridRowHyperlinkExtender.registerClass('Ace.GridRowHyperlinkExtender', Sys.UI.Control);

Sys.Application.notifyScriptLoaded();
