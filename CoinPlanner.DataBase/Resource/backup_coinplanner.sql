PGDMP                      }            coinplannerdb    17.4    17.4     E           0    0    ENCODING    ENCODING     !   SET client_encoding = 'WIN1251';
                           false            F           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            G           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            H           1262    16450    coinplannerdb    DATABASE     s   CREATE DATABASE coinplannerdb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'ru-RU';
    DROP DATABASE coinplannerdb;
                     postgres    false            �            1259    16486 
   categories    TABLE     e   CREATE TABLE public.categories (
    category_id bigint NOT NULL,
    category_name text NOT NULL
);
    DROP TABLE public.categories;
       public         heap r       postgres    false            �            1259    16578 	   fixations    TABLE     4  CREATE TABLE public.fixations (
    fix_id uuid NOT NULL,
    fix_plan_id uuid NOT NULL,
    fix_name text NOT NULL,
    fix_type_id bigint NOT NULL,
    fix_category_id bigint NOT NULL,
    fix_sum real NOT NULL,
    fix_completed boolean NOT NULL,
    fix_next_date timestamp without time zone NOT NULL
);
    DROP TABLE public.fixations;
       public         heap r       postgres    false            �            1259    16585    marks    TABLE     �   CREATE TABLE public.marks (
    mark_id uuid NOT NULL,
    mark_plan_id uuid NOT NULL,
    mark_name text NOT NULL,
    mark_date timestamp without time zone NOT NULL
);
    DROP TABLE public.marks;
       public         heap r       postgres    false            �            1259    16592 
   operations    TABLE     =  CREATE TABLE public.operations (
    oper_id uuid NOT NULL,
    oper_plan_id uuid NOT NULL,
    oper_name text NOT NULL,
    oper_type_id bigint NOT NULL,
    oper_category_id bigint NOT NULL,
    oper_sum real NOT NULL,
    oper_completed boolean NOT NULL,
    oper_next_date timestamp without time zone NOT NULL
);
    DROP TABLE public.operations;
       public         heap r       postgres    false            �            1259    16599    plans    TABLE     �   CREATE TABLE public.plans (
    plan_id uuid NOT NULL,
    plan_name text NOT NULL,
    date_create timestamp without time zone NOT NULL,
    date_update timestamp without time zone NOT NULL
);
    DROP TABLE public.plans;
       public         heap r       postgres    false            �            1259    16498    type_operations    TABLE     b   CREATE TABLE public.type_operations (
    type_id bigint NOT NULL,
    type_name text NOT NULL
);
 #   DROP TABLE public.type_operations;
       public         heap r       postgres    false            =          0    16486 
   categories 
   TABLE DATA           @   COPY public.categories (category_id, category_name) FROM stdin;
    public               postgres    false    217   �%       ?          0    16578 	   fixations 
   TABLE DATA           �   COPY public.fixations (fix_id, fix_plan_id, fix_name, fix_type_id, fix_category_id, fix_sum, fix_completed, fix_next_date) FROM stdin;
    public               postgres    false    219   �&       @          0    16585    marks 
   TABLE DATA           L   COPY public.marks (mark_id, mark_plan_id, mark_name, mark_date) FROM stdin;
    public               postgres    false    220   �&       A          0    16592 
   operations 
   TABLE DATA           �   COPY public.operations (oper_id, oper_plan_id, oper_name, oper_type_id, oper_category_id, oper_sum, oper_completed, oper_next_date) FROM stdin;
    public               postgres    false    221   �&       B          0    16599    plans 
   TABLE DATA           M   COPY public.plans (plan_id, plan_name, date_create, date_update) FROM stdin;
    public               postgres    false    222   '       >          0    16498    type_operations 
   TABLE DATA           =   COPY public.type_operations (type_id, type_name) FROM stdin;
    public               postgres    false    218   '       �           2606    16492    categories categories_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (category_id);
 D   ALTER TABLE ONLY public.categories DROP CONSTRAINT categories_pkey;
       public                 postgres    false    217            �           2606    16584    fixations fixations_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.fixations
    ADD CONSTRAINT fixations_pkey PRIMARY KEY (fix_id);
 B   ALTER TABLE ONLY public.fixations DROP CONSTRAINT fixations_pkey;
       public                 postgres    false    219            �           2606    16591    marks marks_pkey 
   CONSTRAINT     S   ALTER TABLE ONLY public.marks
    ADD CONSTRAINT marks_pkey PRIMARY KEY (mark_id);
 :   ALTER TABLE ONLY public.marks DROP CONSTRAINT marks_pkey;
       public                 postgres    false    220            �           2606    16598    operations operations_pkey 
   CONSTRAINT     ]   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_pkey PRIMARY KEY (oper_id);
 D   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_pkey;
       public                 postgres    false    221            �           2606    16605    plans plans_pkey 
   CONSTRAINT     S   ALTER TABLE ONLY public.plans
    ADD CONSTRAINT plans_pkey PRIMARY KEY (plan_id);
 :   ALTER TABLE ONLY public.plans DROP CONSTRAINT plans_pkey;
       public                 postgres    false    222            �           2606    16504 $   type_operations type_operations_pkey 
   CONSTRAINT     g   ALTER TABLE ONLY public.type_operations
    ADD CONSTRAINT type_operations_pkey PRIMARY KEY (type_id);
 N   ALTER TABLE ONLY public.type_operations DROP CONSTRAINT type_operations_pkey;
       public                 postgres    false    218            �           2606    16636 (   fixations fixations_fix_category_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.fixations
    ADD CONSTRAINT fixations_fix_category_id_fkey FOREIGN KEY (fix_category_id) REFERENCES public.categories(category_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 R   ALTER TABLE ONLY public.fixations DROP CONSTRAINT fixations_fix_category_id_fkey;
       public               postgres    false    217    4762    219            �           2606    16626 $   fixations fixations_fix_plan_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.fixations
    ADD CONSTRAINT fixations_fix_plan_id_fkey FOREIGN KEY (fix_plan_id) REFERENCES public.plans(plan_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 N   ALTER TABLE ONLY public.fixations DROP CONSTRAINT fixations_fix_plan_id_fkey;
       public               postgres    false    222    219    4772            �           2606    16631 $   fixations fixations_fix_type_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.fixations
    ADD CONSTRAINT fixations_fix_type_id_fkey FOREIGN KEY (fix_type_id) REFERENCES public.type_operations(type_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 N   ALTER TABLE ONLY public.fixations DROP CONSTRAINT fixations_fix_type_id_fkey;
       public               postgres    false    219    4764    218            �           2606    16621    marks marks_mark_plan_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.marks
    ADD CONSTRAINT marks_mark_plan_id_fkey FOREIGN KEY (mark_plan_id) REFERENCES public.plans(plan_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 G   ALTER TABLE ONLY public.marks DROP CONSTRAINT marks_mark_plan_id_fkey;
       public               postgres    false    222    4772    220            �           2606    16616 +   operations operations_oper_category_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_oper_category_id_fkey FOREIGN KEY (oper_category_id) REFERENCES public.categories(category_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 U   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_oper_category_id_fkey;
       public               postgres    false    4762    217    221            �           2606    16606 '   operations operations_oper_plan_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_oper_plan_id_fkey FOREIGN KEY (oper_plan_id) REFERENCES public.plans(plan_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 Q   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_oper_plan_id_fkey;
       public               postgres    false    4772    221    222            �           2606    16611 '   operations operations_oper_type_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.operations
    ADD CONSTRAINT operations_oper_type_id_fkey FOREIGN KEY (oper_type_id) REFERENCES public.type_operations(type_id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
 Q   ALTER TABLE ONLY public.operations DROP CONSTRAINT operations_oper_type_id_fkey;
       public               postgres    false    4764    221    218            =   �   x��An1�3�� ��䱹E�Aܑ��#{�C�E�kK��=�5�m��$�����\��F��_�}��yJK���	<����9ب9a����}�t�ʡE�yA߈y��J�(�JN	���4��hS5}��U�ze7����������?������Mh	����i'�I�錾J�e�Ur���������      ?      x������ � �      @      x������ � �      A      x������ � �      B   S   x�Uȹ�0 �:���?{��sS"������2��Hd����y��1i禭�px��~�3P$ŨH6%u����\� >і�      >   &   x� ��2	������
1	����������
\.


�V�     