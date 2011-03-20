Type.registerNamespace('Ace');

// constructor
Ace.FocusBehavior = function (element) 
{
    Ace.FocusBehavior.initializeBase(this, [element]);

    this.HighlightCssClass = null;
    this.NormalCssClass = null;
}

Ace.FocusBehavior.prototype.initialize = function () 
{
    Ace.FocusBehavior.callBaseMethod(this, 'initialize');

    $addHandlers(this.get_element(), { 'mouseover': this.onFocus, 'mouseout': this.onBlur }, this);

    this.get_element().className = this.NormalCssClass;
}

Ace.FocusBehavior.prototype.dispose = function () 
{
    $clearHandlers(this.get_element());

    Ace.FocusBehavior.callBaseMethod(this, 'dispose');
}

Ace.FocusBehavior.prototype.onFocus = function ()
{
    if (this.get_element() && !this.get_element().disabled) 
    {
        this.get_element().className = this.HighlightCssClass;
    }
}

Ace.FocusBehavior.prototype.onBlur = function () 
{
    if (this.get_element() && !this.get_element().disabled)
    {
        this.get_element().className = this.NormalCssClass;
    }
}

Ace.FocusBehavior.prototype.get_HighLightCssClass = function () 
{
    return this.HighlightCssClass;
}

Ace.FocusBehavior.prototype.set_HighLightCssClass = function (value) 
{
    if (this.HighlightCssClass != value) 
    {
        this.HighlightCssClass = value;
        this.raisePropertyChanged('HighlightCssClass');
    }
}

Ace.FocusBehavior.prototype.get_NormalCssClass = function ()
 {
    return this.NormalCssClass;
}

Ace.FocusBehavior.prototype.set_NormalCssClass = function (value) 
{
    if (this.NormalCssClass != value) 
    {
        this.NormalCssClass = value;
        this.raisePropertyChanged('NormalCssClass');
    }
}

Ace.FocusBehavior.registerClass('Ace.FocusBehavior', Sys.UI.Behavior);

Sys.Application.notifyScriptLoaded();
