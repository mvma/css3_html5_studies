$(document).ready(function() {
    var items = $('#gallery li'),
        itemsByTags = [];
    
    items.each(function(index, e) {
        let el = $(e),
            tags = el.data('tags').split(',');
        
        el.attr('data-id', index);

        $.each(tags, function(index, tag) {
            tag = tag.trim();
            
            if(!(tag in itemsByTags))
            {
                itemsByTags[tag] = [];
            }   

            itemsByTags[tag].push(el); 
        });
    });
});