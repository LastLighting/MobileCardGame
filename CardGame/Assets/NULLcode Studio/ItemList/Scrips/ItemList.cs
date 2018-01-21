// NULLcode Studio © 2016
// null-code.ru

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemList : MonoBehaviour {

	public ScrollRect scroll;
	public RectTransform element; // кнопка из которой будет составлен список
	public int offset = 10; // расстояние между элементами

	private Vector2 delta;
	private Vector2 e_Pos;
	private List<RectTransform> buttons;
	private int size;
	private float curY, vPos;

	void Awake()
	{
		buttons = new List<RectTransform>();
		delta = element.sizeDelta;
		delta.y += offset;
		e_Pos = new Vector2(0, -delta.y / 2);
	}

	void ButtonPressed(string id, string title)
	{
		Debug.Log(this + " Выбран элемент списка -> '" + title + "'. Идентификатор объекта: " + id);
	}

	void ButtonRemoved(string id, string title)
	{
		Debug.Log(this + " Удален элемент списка -> '" + title + "'. Идентификатор объекта: " + id);
	}

	public void ClearItemList()
	{
		size = 0;
		foreach(RectTransform b in buttons)
		{
			Destroy(b.gameObject);
		}
		buttons = new List<RectTransform>();
		RectContent();
		Debug.Log(this + " Все элементы списка удалены!");
	}

	void GetCurrentButton(string id) // поиск id и заголовка, нажатой кнопки
	{
		ItemButton item = null;
		foreach(RectTransform b in buttons)
		{
			item = b.GetComponent<ItemButton>();
			if(item.id == id) break;
		}
		ButtonPressed(item.id, item.mainButtonText.text);
	}

	void UpdateList(string id) // функция удаления элемента
	{
		vPos = scroll.verticalNormalizedPosition; // запоминаем позицию скролла
		int j = 0;
		ItemButton item = null;
        bool find = false;
        foreach (RectTransform b in buttons)
        {
            item = b.GetComponent<ItemButton>();
            if (item.id == id)
            {
                if (item.count.text == "2")
                {
                    Sprite sprite = GameObject.Find(item.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite;
                    if (sprite == null)
                    {
                        sprite = Resources.Load("sprites/Cards/power/" + 1, typeof(Sprite)) as Sprite;
                        GameObject.Find(item.id).GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                    }
                    else
                    {
                        sprite = Resources.Load("sprites/Cards/power/" + (Int32.Parse(sprite.name) + 1).ToString(), typeof(Sprite)) as Sprite;
                    }
                    GameObject.Find(item.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite = sprite;
                    item.count.text = null;
                    find = true;
                }                      
            };
        }
        if (!find)
        {
            foreach (RectTransform b in buttons)
            {
                item = b.GetComponent<ItemButton>();
                if (item.id == id) break; // находим нужный элемент
                j++;
            }
            string title = item.mainButtonText.text; // сохраняем заголовок
            string index = item.id; // сохраняем id
            Destroy(item.gameObject); // удаляем этот элемент из списка
            buttons.RemoveAt(j); // удаляем этот элемент из массива
            curY = 0;
            size--; // минус один элемент
            RectContent(); // пересчитываем размеры окна
            foreach (RectTransform b in buttons) // сдвигаем элементы
            {
                b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
                curY += delta.y;
            }
            scroll.verticalNormalizedPosition = vPos; // возвращаем позицию скролла
            ButtonRemoved(index, title); // вывод конечной информации
            Sprite sprite = GameObject.Find(item.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite;
            if(sprite == null)
            {
                sprite = Resources.Load("sprites/Cards/power/" + 1, typeof(Sprite)) as Sprite;
                GameObject.Find(item.id).GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                sprite = Resources.Load("sprites/Cards/power/" + (Int32.Parse(sprite.name) + 1).ToString(), typeof(Sprite)) as Sprite;
            }
            GameObject.Find(item.id).transform.Find("Count").GetComponent<SpriteRenderer>().sprite = sprite;
        }   
	}

	void SetMainButton(Button button, string value) // настройка функций при нажатии на главную кнопку
	{
		button.onClick.AddListener(() => GetCurrentButton(value));
	}

	void SetRemoveButton(Button button, string value) // настройка функций при нажатии на кнопку удаления
	{
		button.onClick.AddListener(() => UpdateList(value));
	}

	// добавление нового элемента в список
	// id - идентификатор объекта который нужно сохранить
	// text - текст для основной кнопки (имя объекта)
	// resetScrollbar - сбросить или нет, позицию скролла
	public void AddToList(string id, string text, bool resetScrollbar)
	{
        bool find = false;
        ItemButton item = null;
        foreach (RectTransform b in buttons)
        {
            item = b.GetComponent<ItemButton>();
            if (item.id == id)
            {
                if (item.count.text != "2")
                {
                    item.count.text = "2";
                }
                else
                {

                }
                find = true;
            };
        }
        if (!find)
        {
            String count = null;
            element.gameObject.SetActive(true);
            vPos = scroll.verticalNormalizedPosition;
            curY = 0;
            size++;
            RectContent();
            foreach (RectTransform b in buttons)
            {
                b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
                curY += delta.y;
            }
            BuildElement(id, text, count);
            if (!resetScrollbar) scroll.verticalNormalizedPosition = vPos;
            element.gameObject.SetActive(false);
        }
	}

	void BuildElement(string id, string text, string count) // создание нового элемента и настройка параметров
	{
		RectTransform clone = Instantiate(element) as RectTransform;
        clone.name = id;
		clone.SetParent(scroll.content);
		clone.localScale = Vector3.one;
		clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
		ItemButton item = clone.GetComponent<ItemButton>();
		SetMainButton(item.mainButton, id);
		SetRemoveButton(item.removeButton, id);
		item.mainButtonText.text = text;
		item.id = id;
        item.count.text = count;
		buttons.Add(clone);
	}

	void RectContent() // определение размера окна с элементами
	{
		float height = delta.y * size;
		scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, height);
		scroll.content.anchoredPosition = Vector2.zero;
	}

    public static implicit operator ItemList(GameObject v)
    {
        throw new NotImplementedException();
    }
    
    public List<CardBean> GetList()
    {
        List<CardBean> list = new List<CardBean>();
        ItemButton item = null;
        foreach (RectTransform b in buttons)
        {
	        CardBean card = new CardBean();
            item = b.GetComponent<ItemButton>();
	        card.id = item.id;
            if (item.count.text == "2")
            {
                list.Add(card);
                list.Add(card);
            }
            else
            {
                list.Add(card);
            }          
        }
        return list;
    }
}
