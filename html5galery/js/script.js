$(document).ready(function () {
    var items = $('#gallery li'),
        itemsByTags = {};

    items.each(function (index, e) {
        let li = $(e),
            tags = li.data('tags').split(',');

        li.attr('data-id', index);

        $.each(tags, function (index, tag) {
            tag = tag.trim();

            if (!(tag in itemsByTags)) {
                itemsByTags[tag] = [];
            }

            itemsByTags[tag].push(li);
        });
    });

    create('All Items', itemsByTags);
    
    $.each(itemsByTags, function(menu, li) {
        create(menu, li);
    });

    $('#navbar a').on('click', function (e) {
        let link = $(this);

        link.addClass('active');

        let siblings = link.siblings();
        siblings.removeClass('active');
        
        $('#gallery').quicksand(link.data('list').find('li'));
        
        e.preventDefault();
    });

    $('#navbar a:first').click();

    function create(menu, liCollection) {
        let ul = $('<ul>', {
            'class': 'hidden'
        });
        
        $.each(liCollection, function() {
            let collection = $(this);
            $.each(collection, function(index, e){
                $(e).clone().appendTo(ul);
            });
        });

        ul.appendTo('#gallery');
        
        let a = $('<a>', {
            href:'#',
            html: menu,
            data:{list:ul}
        }).appendTo('#navbar');
    }
});